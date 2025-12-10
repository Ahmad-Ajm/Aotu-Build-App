قالب
# Notification Types

أنواع الإشعارات (Notifications) المستخدمة في النظام.

---

## قالب Notification Type

### [NotificationType]

**الوصف**: [وصف نوع الإشعار ومتى يُرسل]

**Trigger**: [الحدث الذي يُطلق هذا الإشعار]

**Channels**: Email / SMS / Push / In-App

**Priority**: High / Medium / Low

**Recipients**:
- [نوع المستلمين]
- [شروط الاستلام]

**Template Variables**:
| Variable | Type | Description |
|----------|------|-------------|
| {userName} | string | اسم المستخدم |
| {[variable]} | [type] | [الوصف] |

**Email Template**:
- **Subject**: [الموضوع]
- **Body**: [المحتوى]
- **Layout**: [Layout Template]

**SMS Template**:
- **Message**: [النص (max 160 chars)]

**Push Notification**:
- **Title**: [العنوان]
- **Body**: [المحتوى]
- **Icon**: [مسار الأيقونة]
- **Action**: [الإجراء عند النقر]

**In-App Notification**:
- **Title**: [العنوان]
- **Message**: [الرسالة]
- **Type**: Info / Success / Warning / Error
- **Action URL**: [رابط]

**Frequency Limits**:
- [حدود الإرسال لتجنب الإزعاج]

**Opt-out**: Yes / No
[هل يمكن للمستخدم إيقاف هذا النوع من الإشعارات]

---

## قائمة Notification Types

### User Account Notifications

#### WelcomeNotification
- **Trigger**: عند تسجيل مستخدم جديد
- **Channels**: Email, In-App
- **Priority**: High

#### PasswordResetNotification
- **Trigger**: عند طلب إعادة تعيين كلمة المرور
- **Channels**: Email, SMS
- **Priority**: High

#### AccountLocked Notification
- **Trigger**: عند قفل الحساب
- **Channels**: Email, SMS
- **Priority**: High

### Transaction Notifications

#### [Transaction]CreatedNotification
- **Trigger**: عند إنشاء معاملة
- **Channels**: Email, In-App, Push
- **Priority**: Medium

#### [Transaction]ApprovedNotification
- **Trigger**: عند الموافقة
- **Channels**: Email, SMS, In-App, Push
- **Priority**: High

#### Payment ReceivedNotification
- **Trigger**: عند استلام دفعة
- **Channels**: Email, SMS, In-App
- **Priority**: High

### System Notifications

#### SystemMaintenanceNotification
- **Trigger**: قبل صيانة النظام
- **Channels**: Email, In-App, Push
- **Priority**: High

#### DataBackupCompleted
- **Trigger**: بعد نسخ احتياطي ناجح
- **Channels**: Email (Admins only)
- **Priority**: Low

### Reminder Notifications

#### [Action]ReminderNotification
- **Trigger**: قبل موعد معين
- **Channels**: Email, Push
- **Priority**: Medium
- **Schedule**: 24h, 1h before

---

## Notification Channels

### Email
- **Provider**: SMTP / SendGrid / AWS SES
- **Templates**: Razor / Liquid
- **Attachments**: Supported
- **Tracking**: Open rate, Click rate

### SMS
- **Provider**: Twilio / Nexmo
- **Max Length**: 160 characters (160 حرف)
- **Cost**: $$$ (high)
- **Delivery Rate**: ~98%

### Push Notifications
- **Provider**: Firebase Cloud Messaging (FCM)
- **Platforms**: iOS, Android, Web
- **Rich Content**: Images, Actions

### In-App Notifications
- **Storage**: Database
- **Real-time**: SignalR
- **Mark as Read**: Yes
- **Retention**: 30 days

---

## Notification Preferences

Users يمكنهم التحكم في:
- تفعيل/إيقاف كل نوع
- اختيار القنوات (Email, SMS, Push)
- تحديد أوقات الإرسال
- Frequency (كل إشعار / يومي digest / أسبوعي)

---

## Batching & Throttling

### Batching
- تجميع إشعارات متعددة في رسالة واحدة
- مثال: Daily Digest Email

### Throttling
- الحد الأقصى للإشعارات لكل مستخدم:
  - Email: 10 / hour
  - SMS: 5 / hour
  - Push: 20 / hour

---

## Notification Tracking

| Metric | Description |
|--------|-------------|
| Sent | عدد الإشعارات المُرسلة |
| Delivered | عدد الوصول الناجح |
| Opened | عدد الفتح (Email) |
| Clicked | عدد النقر على الروابط |
| Failed | عدد الفشل |
| Bounced | عدد الارتداد (Email) |

---

## Best Practices

1. **Personalization**: استخدام اسم المستخدم وبيانات شخصية
2. **Timing**: إرسال في الأوقات المناسبة
3. **Relevance**: إشعارات ذات صلة فقط
4. **Opt-out**: السماح بإيقاف الإشعارات
5. **Testing**: اختبار القوالب قبل الإرسال

---

## ملاحظات التنفيذ

- استخدام ABP Notification System
- Queue System للإرسال الجماعي
- Retry Logic للفشل المؤقت
- Logging لجميع الإشعارات
- GDPR Compliance للبيانات الشخصية

