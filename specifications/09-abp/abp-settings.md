قالب
# ABP Settings

إعدادات التطبيق (Settings) في ABP Framework.

---

## مفهوم Settings

Settings في ABP هي:
- إعدادات قابلة للتغيير بدون إعادة النشر
- يمكن تحديدها على مستوى: Global, Tenant, User
- مخزنة في Database
- Cached للأداء

---

## Setting Scopes

| Scope | Description | Example |
|-------|-------------|---------|
| Global | إعدادات عامة للتطبيق | SMTP Settings |
| Tenant | إعدادات خاصة بـ Tenant | Company Logo |
| User | إعدادات خاصة بمستخدم | Language Preference |

---

## Setting Definitions

### قالب Setting

```csharp
public class [ModuleName]SettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                "[SettingName]",
                "[DefaultValue]",
                displayName: LocalizableString.Create<[Resource]>("[DisplayName]"),
                description: LocalizableString.Create<[Resource]>>("[Description]"),
                scopes: SettingScopes.Global | SettingScopes.Tenant
            )
        );
    }
}
```

---

## قائمة Settings

### Global Settings

#### Email Settings
```
App.Email.SmtpHost: "smtp.gmail.com"
App.Email.SmtpPort: "587"
App.Email.SmtpUserName: ""
App.Email.SmtpPassword: "" (Encrypted)
App.Email.SmtpEnableSsl: "true"
App.Email.DefaultFromAddress: "noreply@example.com"
App.Email.DefaultFromDisplayName: "App Name"
```

#### General Settings
```
App.General.Timezone: "Asia/Riyadh"
App.General.DefaultLanguage: "ar"
App.General.DefaultCurrency: "SAR"
App.General.DateFormat: "yyyy-MM-dd"
App.General.TimeFormat: "HH:mm"
```

#### Security Settings
```
App.Security.PasswordComplexity.RequireDigit: "true"
App.Security.PasswordComplexity.RequireLowercase: "true"
App.Security.PasswordComplexity.RequireUppercase: "true"
App.Security.PasswordComplexity.RequireNonAlphanumeric: "true"
App.Security.PasswordComplexity.RequiredLength: "8"
App.Security.Lockout.MaxFailedAccessAttempts: "5"
App.Security.Lockout.LockoutDuration: "300" (seconds)
```

#### File Upload Settings
```
App.FileUpload.MaxFileSize: "10485760" (10 MB in bytes)
App.FileUpload.AllowedExtensions: ".jpg,.jpeg,.png,.pdf,.docx"
App.FileUpload.StorageProvider: "AzureBlob" (or "Local", "S3")
```

### Tenant Settings

#### Branding
```
App.Tenant.Branding.LogoUrl: ""
App.Tenant.Branding.PrimaryColor: "#1890ff"
App.Tenant.Branding.CompanyName: ""
```

#### Business Settings
```
App.Tenant.Business.TaxRate: "15"
App.Tenant.Business.TaxNumber: ""
App.Tenant.Business.Address: ""
```

### User Settings

#### Preferences
```
App.User.Preferences.Language: "ar"
App.User.Preferences.Theme: "light" (or "dark")
App.User.Preferences.TimeZone: "Asia/Riyadh"
App.User.Preferences.DateFormat: "dd/MM/yyyy"
```

#### Notifications
```
App.User.Notifications.Email.Enabled: "true"
App.User.Notifications.SMS.Enabled: "false"
App.User.Notifications.Push.Enabled: "true"
```

---

## Setting Management

### قراءة Setting

```csharp
// Get setting value
var smtpHost = await SettingProvider.GetOrNullAsync("App.Email.SmtpHost");

// Get with default
var maxFileSize = await SettingProvider.GetAsync<long>(
    "App.FileUpload.MaxFileSize", 
    defaultValue: 5242880
);

// Get for tenant
var logo = await SettingProvider.GetOrNullForTenantAsync(
    "App.Tenant.Branding.LogoUrl", 
    tenantId
);

// Get for user
var language = await SettingProvider.GetOrNullForUserAsync(
    "App.User.Preferences.Language", 
    userId
);
```

### كتابة Setting

```csharp
// Set global setting
await SettingManager.SetGlobalAsync("App.Email.SmtpHost", "smtp.example.com");

// Set for tenant
await SettingManager.SetForTenantAsync(
    tenantId,
    "App.Tenant.Branding.LogoUrl",
    "https://cdn.example.com/logo.png"
);

// Set for user
await SettingManager.SetForUserAsync(
    userId,
    "App.User.Preferences.Language",
    "en"
);
```

---

## Setting Encryption

إعدادات حساسة (مثل Passwords) يجب تشفيرها:

```csharp
context.Add(
    new SettingDefinition(
        "App.Email.SmtpPassword",
        isEncrypted: true
    )
);
```

---

## Setting UI

### Settings Page
- **URL**: `/settings`
- **Tabs**: General, Email, Security, File Upload, ...
- **Permissions**: Admin only

### Angular Component
```typescript
export class SettingsComponent {
  async loadSettings() {
    this.settings = await this.settingService.getAll();
  }
  
  async saveSettings() {
    await this.settingService.update(this.settings);
  }
}
```

---

## Setting Groups

تجميع Settings في Groups للتنظيم:

```csharp
var emailGroup = context.AddGroup("Email", 
    displayName: LocalizableString.Create<Resource>("Email"));

emailGroup.AddSetting(new SettingDefinition("App.Email.SmtpHost", ""));
emailGroup.AddSetting(new SettingDefinition("App.Email.SmtpPort", "587"));
```

---

## Default Values

### Strategies

#### Hard-coded Default
```csharp
new SettingDefinition("App.Email.SmtpPort", "587")
```

#### من Configuration
```csharp
var defaultPort = _configuration["Email:SmtpPort"] ?? "587";
new SettingDefinition("App.Email.SmtpPort", defaultPort)
```

#### Computed Default
```csharp
var defaultTimezone = TimeZoneInfo.Local.Id;
new SettingDefinition("App.General.Timezone", defaultTimezone)
```

---

## Setting Inheritance

Settings لها Inheritance hierarchy:

```
Global → Tenant → User
```

- إذا User لم يُحدد Setting، يُستخدم Tenant Setting
- إذا Tenant لم يُحدد، يُستخدم Global Setting

---

## Best Practices

1. **Naming**: استخدام Hierarchical names: `App.[Module].[Category].[SettingName]`
2. **Defaults**: تحديد قيم افتراضية معقولة
3. **Validation**: التحقق من القيم عند التحديث
4. **Encryption**: للإعدادات الحساسة
5. **Caching**: Settings تُخزن في Cache تلقائياً
6. **Documentation**: توثيق كل Setting وتأثيره

---

## Migration من appsettings.json إلى Database

بعض الإعدادات يمكن نقلها:

| appsettings.json | Database Setting |
|------------------|------------------|
| Email:SmtpHost | App.Email.SmtpHost |
| FileUpload:MaxSize | App.FileUpload.MaxFileSize |

**متى نستخدم appsettings.json vs Database Settings**:
- **appsettings.json**: إعدادات البنية التحتية (ConnectionStrings, Logging)
- **Database Settings**: إعدادات الأعمال القابلة للتغيير بدون Deployment

---

## ملاحظات التنفيذ

- Settings مدمجة في ABP Framework
- Database: AbpSettings
- Caching: Distributed Cache للأداء
- Multi-tenancy: كل Tenant له Settings خاصة
- UI: Setting Management Module جاهز

