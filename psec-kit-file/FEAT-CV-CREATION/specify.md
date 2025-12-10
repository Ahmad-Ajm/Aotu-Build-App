# مواصفات تفصيلية لميزة إنشاء السيرة الذاتية

## 1. نماذج البيانات (Data Models)

### 1.1 نموذج السيرة الذاتية (CV)
```csharp
public class CV
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public Guid TemplateId { get; set; }
    public CVStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string PreviewImageUrl { get; set; }
    
    // Navigation properties
    public virtual ICollection<CVSection> Sections { get; set; }
    public virtual CVTemplate Template { get; set; }
}

public enum CVStatus
{
    Draft,
    Published,
    Archived
}
```

### 1.2 نموذج قسم السيرة الذاتية (CVSection)
```csharp
public class CVSection
{
    public Guid Id { get; set; }
    public Guid CVId { get; set; }
    public SectionType Type { get; set; }
    public int Order { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } // JSON format
    public bool IsVisible { get; set; }
    
    // Navigation property
    public virtual CV CV { get; set; }
}

public enum SectionType
{
    PersonalInfo,
    Summary,
    Education,
    Experience,
    Skills,
    Certifications,
    Languages,
    Projects,
    References,
    Custom
}
```

### 1.3 نموذج القالب (CVTemplate)
```csharp
public class CVTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PreviewImageUrl { get; set; }
    public string TemplateFile { get; set; } // HTML/CSS template
    public bool IsActive { get; set; }
    public bool IsPremium { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public virtual ICollection<CV> CVs { get; set; }
}
```

## 2. واجهات برمجة التطبيقات (APIs)

### 2.1 واجهات إدارة السير الذاتية
```csharp
[Route("api/cv")]
public class CVController : ControllerBase
{
    // GET /api/cv - Get all CVs for current user
    [HttpGet]
    public async Task<IActionResult> GetUserCVs()
    
    // GET /api/cv/{id} - Get specific CV
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCV(Guid id)
    
    // POST /api/cv - Create new CV
    [HttpPost]
    public async Task<IActionResult> CreateCV([FromBody] CreateCVDto dto)
    
    // PUT /api/cv/{id} - Update CV
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCV(Guid id, [FromBody] UpdateCVDto dto)
    
    // DELETE /api/cv/{id} - Delete CV
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCV(Guid id)
    
    // POST /api/cv/{id}/duplicate - Duplicate CV
    [HttpPost("{id}/duplicate")]
    public async Task<IActionResult> DuplicateCV(Guid id)
}
```

### 2.2 واجهات إدارة الأقسام
```csharp
[Route("api/cv/{cvId}/sections")]
public class CVSectionController : ControllerBase
{
    // GET /api/cv/{cvId}/sections - Get all sections
    [HttpGet]
    public async Task<IActionResult> GetSections(Guid cvId)
    
    // POST /api/cv/{cvId}/sections - Add new section
    [HttpPost]
    public async Task<IActionResult> AddSection(Guid cvId, [FromBody] AddSectionDto dto)
    
    // PUT /api/cv/{cvId}/sections/{id} - Update section
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSection(Guid cvId, Guid id, [FromBody] UpdateSectionDto dto)
    
    // DELETE /api/cv/{cvId}/sections/{id} - Delete section
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSection(Guid cvId, Guid id)
    
    // PUT /api/cv/{cvId}/sections/reorder - Reorder sections
    [HttpPut("reorder")]
    public async Task<IActionResult> ReorderSections(Guid cvId, [FromBody] ReorderSectionsDto dto)
}
```

### 2.3 واجهات التصدير
```csharp
[Route("api/cv/{id}/export")]
public class CVExportController : ControllerBase
{
    // GET /api/cv/{id}/export/pdf - Export to PDF
    [HttpGet("pdf")]
    public async Task<IActionResult> ExportToPdf(Guid id)
    
    // GET /api/cv/{id}/export/docx - Export to DOCX
    [HttpGet("docx")]
    public async Task<IActionResult> ExportToDocx(Guid id)
    
    // GET /api/cv/{id}/export/html - Export to HTML
    [HttpGet("html")]
    public async Task<IActionResult> ExportToHtml(Guid id)
}
```

## 3. واجهة المستخدم (UI Components)

### 3.1 مكونات Angular الرئيسية

#### CV Editor Component
```typescript
@Component({
  selector: 'app-cv-editor',
  templateUrl: './cv-editor.component.html',
  styleUrls: ['./cv-editor.component.scss']
})
export class CVEditorComponent {
  cv: CV;
  sections: CVSection[];
  currentTemplate: CVTemplate;
  
  // Methods
  addSection(type: SectionType): void;
  removeSection(sectionId: string): void;
  updateSection(sectionId: string, content: any): void;
  reorderSections(newOrder: string[]): void;
  saveCV(): void;
  previewCV(): void;
  exportCV(format: 'pdf' | 'docx' | 'html'): void;
}
```

#### Template Gallery Component
```typescript
@Component({
  selector: 'app-template-gallery',
  templateUrl: './template-gallery.component.html',
  styleUrls: ['./template-gallery.component.scss']
})
export class TemplateGalleryComponent {
  templates: CVTemplate[];
  selectedTemplate: CVTemplate;
  
  // Methods
  selectTemplate(template: CVTemplate): void;
  filterTemplates(category: string): void;
  searchTemplates(query: string): void;
}
```

#### Section Editor Component
```typescript
@Component({
  selector: 'app-section-editor',
  templateUrl: './section-editor.component.html',
  styleUrls: ['./section-editor.component.scss']
})
export class SectionEditorComponent {
  @Input() section: CVSection;
  @Output() sectionUpdated = new EventEmitter<any>();
  
  // Methods
  updateContent(content: any): void;
  toggleVisibility(): void;
}
```

## 4. تدفقات العمل (Workflows)

### 4.1 إنشاء سيرة ذاتية جديدة
1. المستخدم ينقر على "إنشاء سيرة ذاتية جديدة"
2. يعرض معرض القوالب
3. المستخدم يختار قالباً
4. يفتح محرر السيرة الذاتية مع القالب المحدد
5. المستخدم يبدأ بإضافة المحتوى

### 4.2 تحرير سيرة ذاتية موجودة
1. المستخدم يختار سيرة ذاتية من القائمة
2. يفتح محرر السيرة الذاتية مع المحتوى الحالي
3. المستخدم يقوم بالتعديلات
4. التغييرات تحفظ تلقائياً أو يدوياً

### 4.3 تصدير السيرة الذاتية
1. المستخدم ينقر على "تصدير"
2. يختار الصيغة المطلوبة (PDF، DOCX، HTML)
3. النظام يقوم بمعالجة القالب والمحتوى
4. يتم تحميل الملف الناتج

## 5. التصميم والواجهة

### 5.1 تخطيط الصفحات
- **صفحة رئيسية**: قائمة السير الذاتية + زر إنشاء جديد
- **صفحة المحرر**: شريط أدوات + معاينة + قائمة الأقسام
- **صفحة المعاينة**: عرض كامل للسيرة الذاتية
- **صفحة التصدير**: خيارات التصدير والإعدادات

### 5.2 المكونات البصرية
- **شريط الأدوات**: أزرار الحفظ، المعاينة، التصدير
- **قائمة الأقسام**: قائمة قابلة للسحب والإفلات
- **محرر النص**: محرر WYSIWYG مع تنسيقات
- **معاينة مباشرة**: تحديث فوري للتغييرات

## 6. الاعتبارات التقنية

### 6.1 الأداء
- التخزين المؤقت للقوالب
- التحميل البطيء للمكونات
- تحسين استعلامات قاعدة البيانات
- ضغط الصور والمحتوى

### 6.2 الأمان
- التحقق من ملكية السيرة الذاتية
- حماية واجهات برمجة التطبيقات
- التحقق من صحة بيانات الإدخال
- تشفير البيانات الحساسة

### 6.3 قابلية التوسع
- تصميم معماري قابل للتوسع
- فصل الخدمات
- استخدام قوائم الانتظار للمهام الثقيلة
- دعم التحميل المتعدد