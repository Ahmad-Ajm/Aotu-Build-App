# مواصفات تفصيلية لميزة إنشاء السيرة الذاتية (FEAT-CV-CREATION)

## 📊 نظرة عامة تقنية

### الهيكل التقني
```
Frontend (Angular 15+)
├── Components/
│   ├── cv-creator/          # الصفحة الرئيسية
│   ├── personal-info/       # المعلومات الشخصية
│   ├── experience/          # الخبرات العملية
│   ├── education/           # المؤهلات التعليمية
│   ├── skills/              # المهارات
│   ├── projects/            # المشاريع والإنجازات
│   ├── sections-panel/      # لوحة الأقسام
│   └── preview-panel/       # لوحة المعاينة
├── Services/
│   ├── cv.service.ts        # خدمة إدارة السير الذاتية
│   ├── template.service.ts  # خدمة القوالب
│   └── export.service.ts    # خدمة التصدير
└── Models/
    ├── cv.model.ts          # نموذج السيرة الذاتية
    ├── section.model.ts     # نموذج القسم
    └── template.model.ts    # نموذج القالب

Backend (ABP Framework .NET 7)
├── Application/
│   ├── CvAppService.cs      # خدمة التطبيق
│   └── DTOs/                # كائنات نقل البيانات
├── Domain/
│   ├── Entities/            # الكيانات
│   └── Repositories/        # المستودعات
└── Infrastructure/
    └── Database/            # تكوين قاعدة البيانات
```

## 🎨 مواصفات واجهة المستخدم

### صفحة إنشاء السيرة الذاتية (`/cv/create`)
```typescript
interface CvCreatorPage {
  // الهيكل العام
  header: {
    title: string;           // "إنشاء سيرة ذاتية جديدة"
    saveButton: Button;      // زر الحفظ
    exportButton: Button;    // زر التصدير
    previewButton: Button;   // زر المعاينة
  };
  
  // منطقة العمل الرئيسية
  workspace: {
    leftPanel: SectionsPanel;    // لوحة الأقسام (25%)
    centerPanel: EditorPanel;    // لوحة المحرر (50%)
    rightPanel: PreviewPanel;    // لوحة المعاينة (25%)
  };
  
  // الحالة
  state: {
    currentCv: CvModel;          // السيرة الذاتية الحالية
    isDirty: boolean;            // هل هناك تغييرات غير محفوظة
    isLoading: boolean;          // حالة التحميل
    selectedSection: string;     // القسم المحدد
  };
}
```

### نموذج السيرة الذاتية (CvModel)
```typescript
interface CvModel {
  id: string;                    // المعرف الفريد
  userId: string;                // معرف المستخدم
  title: string;                 // عنوان السيرة الذاتية
  personalInfo: PersonalInfo;    // المعلومات الشخصية
  sections: CvSection[];         // الأقسام
  template: TemplateConfig;      // إعدادات القالب
  createdAt: Date;               // تاريخ الإنشاء
  updatedAt: Date;               // تاريخ التحديث
  isPublished: boolean;          // هل تم نشرها
}

interface PersonalInfo {
  fullName: string;              // الاسم الكامل
  email: string;                 // البريد الإلكتروني
  phone: string;                 | // رقم الهاتف
  address?: string;              // العنوان (اختياري)
  linkedinUrl?: string;          // رابط LinkedIn (اختياري)
  githubUrl?: string;            // رابط GitHub (اختياري)
  summary?: string;              // ملخص شخصي (اختياري)
}

interface CvSection {
  id: string;                    // معرف القسم
  type: SectionType;             // نوع القسم
  title: string;                 // عنوان القسم
  content: any;                  // محتوى القسم
  order: number;                 | // ترتيب العرض
  isVisible: boolean;            // هل القسم مرئي
}

enum SectionType {
  PERSONAL_INFO = 'personal_info',
  EXPERIENCE = 'experience',
  EDUCATION = 'education',
  SKILLS = 'skills',
  PROJECTS = 'projects',
  LANGUAGES = 'languages',
  CERTIFICATIONS = 'certifications',
  CUSTOM = 'custom'
}
```

## 🔧 مواصفات API

### نقاط النهاية الرئيسية

#### 1. إدارة السير الذاتية
```csharp
// GET /api/cv - الحصول على قائمة السير الذاتية
[HttpGet]
[Authorize]
Task<List<CvDto>> GetUserCvs();

// GET /api/cv/{id} - الحصول على سيرة ذاتية محددة
[HttpGet("{id}")]
[Authorize]
Task<CvDto> GetCv(Guid id);

// POST /api/cv - إنشاء سيرة ذاتية جديدة
[HttpPost]
[Authorize]
Task<CvDto> CreateCv([FromBody] CreateCvDto input);

// PUT /api/cv/{id} - تحديث سيرة ذاتية
[HttpPut("{id}")]
[Authorize]
Task<CvDto> UpdateCv(Guid id, [FromBody] UpdateCvDto input);

// DELETE /api/cv/{id} - حذف سيرة ذاتية
[HttpDelete("{id}")]
[Authorize]
Task DeleteCv(Guid id);
```

#### 2. إدارة الأقسام
```csharp
// POST /api/cv/{cvId}/sections - إضافة قسم جديد
[HttpPost("{cvId}/sections")]
[Authorize]
Task<SectionDto> AddSection(Guid cvId, [FromBody] AddSectionDto input);

// PUT /api/cv/{cvId}/sections/{sectionId} - تحديث قسم
[HttpPut("{cvId}/sections/{sectionId}")]
[Authorize]
Task<SectionDto> UpdateSection(Guid cvId, Guid sectionId, [FromBody] UpdateSectionDto input);

// DELETE /api/cv/{cvId}/sections/{sectionId} - حذف قسم
[HttpDelete("{cvId}/sections/{sectionId}")]
[Authorize]
Task DeleteSection(Guid cvId, Guid sectionId);

// PUT /api/cv/{cvId}/sections/order - تحديث ترتيب الأقسام
[HttpPut("{cvId}/sections/order")]
[Authorize]
Task UpdateSectionsOrder(Guid cvId, [FromBody] UpdateOrderDto input);
```

## 🗄️ مواصفات قاعدة البيانات

### الجداول الرئيسية

#### جدول CVs
```sql
CREATE TABLE Cvs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    PersonalInfo_Json NVARCHAR(MAX) NOT NULL,
    TemplateId UNIQUEIDENTIFIER NULL,
    IsPublished BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Cvs_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_Cvs_Templates FOREIGN KEY (TemplateId) REFERENCES Templates(Id)
);

CREATE INDEX IX_Cvs_UserId ON Cvs(UserId);
CREATE INDEX IX_Cvs_CreatedAt ON Cvs(CreatedAt);
```

#### جدول Sections
```sql
CREATE TABLE Sections (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CvId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content_Json NVARCHAR(MAX) NOT NULL,
    [Order] INT NOT NULL,
    IsVisible BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Sections_Cvs FOREIGN KEY (CvId) REFERENCES Cvs(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Sections_CvId ON Sections(CvId);
CREATE INDEX IX_Sections_Order ON Sections([Order]);
```

## 🔐 مواصفات الأمان

### التحقق من الصحة (Validation)
```csharp
public class CreateCvDto : IValidatableObject
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; }
    
    [Required]
    public PersonalInfoDto PersonalInfo { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (PersonalInfo != null)
        {
            if (string.IsNullOrEmpty(PersonalInfo.FullName))
                yield return new ValidationResult("الاسم الكامل مطلوب");
                
            if (!IsValidEmail(PersonalInfo.Email))
                yield return new ValidationResult("البريد الإلكتروني غير صالح");
        }
    }
    
    private bool IsValidEmail(string email)
    {
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch {
            return false;
        }
    }
}
```

### التخويل (Authorization)
```csharp
[Authorize(Policy = "CvOwner")]
public class CvAppService : ApplicationService, ICvAppService
{
    // سياسة CvOwner تتحقق من أن المستخدم هو مالك السيرة الذاتية
}

// في Startup.cs أو Module
services.AddAuthorization(options =>
{
    options.AddPolicy("CvOwner", policy =>
        policy.Requirements.Add(new CvOwnerRequirement()));
});
```

## 📱 مواصفات الاستجابة

### نمط الاستجابة الموحد
```typescript
interface ApiResponse<T> {
  success: boolean;          // نجاح/فشل العملية
  data?: T;                  | // البيانات (في حالة النجاح)
  error?: ApiError;          // الخطأ (في حالة الفشل)
  message?: string;          // رسالة توضيحية
  timestamp: string;         // الطابع الزمني
}

interface ApiError {
  code: string;              // كود الخطأ
  message: string;           // رسالة الخطأ
  details?: any;             // تفاصيل إضافية
  validationErrors?: ValidationError[]; // أخطاء التحقق
}

interface ValidationError {
  field: string;             // اسم الحقل
  message: string;           | // رسالة الخطأ
}
```

## 🧪 مواصفات الاختبار

### اختبارات الوحدة (Unit Tests)
```csharp
[TestClass]
public class CvAppServiceTests
{
    [TestMethod]
    public async Task CreateCv_ValidInput_ReturnsCvDto()
    {
        // Arrange
        var input = new CreateCvDto
        {
            Title = "سيرتي الذاتية",
            PersonalInfo = new PersonalInfoDto
            {
                FullName = "أحمد محمد",
                Email = "ahmed@example.com"
            }
        };
        
        // Act
        var result = await _cvAppService.CreateCv(input);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(input.Title, result.Title);
        Assert.AreEqual(input.PersonalInfo.FullName, result.PersonalInfo.FullName);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public async Task CreateCv_InvalidEmail_ThrowsValidationException()
    {
        // Arrange
        var input = new CreateCvDto
        {
            Title = "سيرتي الذاتية",
            PersonalInfo = new PersonalInfoDto
            {
                FullName = "أحمد محمد",
                Email = "invalid-email"
            }
        };
        
        // Act & Assert
        await _cvAppService.CreateCv(input);
    }
}
```

## 📝 ملاحظات تقنية إضافية

### الأداء
1. **التخزين المؤقت**: استخدام Redis للتخزين المؤقت للبيانات المتكررة
2. **التجزئة**: تقسيم قاعدة البيانات إذا تجاوز عدد السير الذاتية مليون سجل
3. **ضغط البيانات**: ضغط JSON قبل التخزين لتقليل المساحة
4. **التحميل البطيء**: تحميل الأقسام عند الحاجة فقط (Lazy Loading)

### قابلية التوسع
1. **الهيكل المعياري**: تصميم المكونات لتكون مستقلة وقابلة لإعادة الاستخدام
2. **واجهات برمجية**: تصميم API لتكون stateless وقابلة للتوسع أفقيًا
3. **قاعدة البيانات**: استخدام أنماط التصميم المناسبة للتوسع المستقبلي

### الصيانة
1. **التوثيق**: توثيق جميع الواجهات البرمجية باستخدام Swagger
2. **التسجيل**: تسجيل جميع العمليات المهمة للتصحيح والمراقبة
3. **النسخ الاحتياطي**: نظام نسخ احتياطي تلقائي للبيانات
4. **المراقبة**: مراقبة الأداء والصحة باستخدام Application Insights