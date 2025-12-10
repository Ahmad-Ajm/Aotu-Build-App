قالب
# ABP Features Configuration

تكوين الميزات (Features) في ABP Framework للتحكم في الوظائف حسب الاشتراكات.

---

## مفهوم Features

Features في ABP تُستخدم لـ:
- التحكم في الميزات حسب الاشتراك (Edition/Tenant)
- تحديد حدود الاستخدام (Quotas)
- تفعيل/تعطيل وظائف معينة

---

## Feature Definitions

### قالب Feature

```csharp
public class [ModuleName]FeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup("[GroupName]");
        
        group.AddFeature(
            "[FeatureName]",
            defaultValue: "true",
            displayName: LocalizableString.Create<[Resource]>("[DisplayName]"),
            description: LocalizableString.Create<[Resource]>("[Description]"),
            valueType: new ToggleStringValueType()
        );
    }
}
```

---

## Feature Value Types

### ToggleStringValueType
- **قيم**: "true" / "false"
- **Use Case**: تفعيل/تعطيل ميزة

### FreeTextStringValueType
- **قيم**: أي نص
- **Use Case**: إعدادات مخصصة

### SelectionStringValueType
- **قيم**: من قائمة محددة
- **Use Case**: اختيار من خيارات

---

## قائمة Features

### Core Features

#### [Module].[Feature].Enable
- **Type**: Toggle
- **Default**: false
- **Description**: تفعيل ميزة [Feature]
- **Usage**:
```csharp
await FeatureChecker.CheckEnabledAsync("[Module].[Feature].Enable");
```

#### [Module].[Entity].MaxCount
- **Type**: FreeText (integer)
- **Default**: "10"
- **Description**: الحد الأقصى لعدد [Entity]
- **Usage**:
```csharp
var maxCount = await FeatureChecker.GetAsync<int>("[Module].[Entity].MaxCount");
```

### Example Features Structure

```
AppFeatures
├── Users
│   ├── Users.Enable (Toggle)
│   ├── Users.MaxUserCount (FreeText)
│   └── Users.AdvancedPermissions (Toggle)
├── [Module]
│   ├── [Module].Enable (Toggle)
│   ├── [Module].[Entity].MaxCount (FreeText)
│   └── [Module].[Feature].Enable (Toggle)
└── Integrations
    ├── Integrations.Email (Toggle)
    ├── Integrations.SMS (Toggle)
    └── Integrations.API.MaxCallsPerDay (FreeText)
```

---

## Editions (Plans)

### قالب Edition

| Edition | Price/Month | Features |
|---------|-------------|----------|
| Free | $0 | Basic features only |
| Starter | $29 | Limited features + quotas |
| Professional | $99 | Most features + higher quotas |
| Enterprise | $299 | All features + unlimited |

### Edition Configuration

```csharp
public class EditionDataSeeder : IDataSeedContributor
{
    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateEditionsAsync();
    }
    
    private async Task CreateEditionsAsync()
    {
        // Free Edition
        var free = new Edition(Guid.NewGuid(), "Free", "Free Plan");
        await SetEditionFeatureValuesAsync(free, new Dictionary<string, string>
        {
            { "Users.MaxUserCount", "5" },
            { "[Module].Enable", "false" },
            { "Integrations.Email", "true" },
            { "Integrations.SMS", "false" }
        });
        
        // Professional Edition
        var pro = new Edition(Guid.NewGuid(), "Professional", "Professional Plan");
        await SetEditionFeatureValuesAsync(pro, new Dictionary<string, string>
        {
            { "Users.MaxUserCount", "100" },
            { "[Module].Enable", "true" },
            { "Integrations.Email", "true" },
            { "Integrations.SMS", "true" }
        });
    }
}
```

---

## Feature Checking

### في Code

```csharp
// Check if feature is enabled
if (await FeatureChecker.IsEnabledAsync("[Module].[Feature].Enable"))
{
    // Feature is enabled
}

// Get feature value
var maxCount = await FeatureChecker.GetAsync<int>("[Module].[Entity].MaxCount");

// Check with exception
await FeatureChecker.CheckEnabledAsync("[Module].[Feature].Enable");
// Throws AbpAuthorizationException if disabled
```

### في UI (Angular)

```typescript
// Check if feature is enabled
if (await this.featureService.getFeatureValue('[Module].[Feature].Enable') === 'true') {
  // Show feature
}
```

### في Authorization

```csharp
[RequiresFeature("[Module].[Feature].Enable")]
public async Task<ActionResult> ProtectedAction()
{
    // This action requires the feature to be enabled
}
```

---

## Tenant-specific Features

Tenants يمكنهم تخصيص Features (بالإضافة لـ Edition):

```csharp
await FeatureManager.SetForTenantAsync(
    tenantId,
    "[Module].[Entity].MaxCount",
    "500" // Override edition default
);
```

---

## Feature UI

### Feature Management Page
- **URL**: `/features`
- **Permissions**: Host only
- **Functionality**:
  - View all features
  - Set feature values per Edition
  - Set feature values per Tenant (override)

---

## Best Practices

1. **Naming**: استخدام Hierarchical names: `[Module].[SubModule].[Feature]`
2. **Defaults**: تحديد قيم افتراضية معقولة
3. **Localization**: ترجمة أسماء وأوصاف Features
4. **Testing**: اختبار جميع السيناريوهات (Enabled/Disabled)
5. **Documentation**: توثيق كل Feature وغرضه

---

## Migration من Permission إلى Feature

بعض الوظائف يمكن نقلها من Permission إلى Feature:

| Permission | Feature |
|------------|---------|
| [Module].Advanced.Create | [Module].Advanced.Enable |
| [Module].BulkOperations | [Module].BulkOperations.Enable |

**متى نستخدم Permission vs Feature**:
- **Permission**: للتحكم في الوصول (من يستطيع؟)
- **Feature**: للتحكم في الوظائف (هل متاحة؟)

---

## ملاحظات التنفيذ

- Features مدمجة في ABP Framework
- Database: AbpFeatures, AbpFeatureValues
- Caching: Feature values تُخزن في Cache
- Multi-tenancy: كل Tenant له Features خاصة

