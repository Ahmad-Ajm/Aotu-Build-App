# مواصفات تفصيلية: إنشاء السيرة الذاتية (FEAT-CV-CREATION)

## 📋 نظرة عامة
هذا المستند يحدد المواصفات التفصيلية لنظام إنشاء وإدارة السير الذاتية.

## 🎯 المتطلبات الوظيفية

### FR-001: إنشاء سيرة ذاتية جديدة
**الوصف:** المستخدم يمكنه إنشاء سيرة ذاتية جديدة من البداية أو باستخدام قالب جاهز.

**المتطلبات:**
1. زر "إنشاء سيرة ذاتية جديدة" في لوحة التحكم
2. اختيار بين:
   - البدء من الصفر
   - استخدام قالب جاهز (مع معاينة)
3. إدخال المعلومات الأساسية:
   - العنوان (مطلوب، 3-200 حرف)
   - الملخص (اختياري، 0-500 حرف)
   - اللغة (العربية/الإنجليزية، افتراضي: العربية)
4. حفظ كمسودة أو نشر مباشرة

**قواعد العمل:**
- كل مستخدم يمكنه إنشاء عدد غير محدود من السير الذاتية
- السيرة الذاتية تحفظ تلقائياً كل 30 ثانية
- حالة السيرة الذاتية: Draft, Published, Archived

### FR-002: إدارة أقسام السيرة الذاتية
**الوصف:** المستخدم يمكنه إضافة وتعديل وحذف أقسام السيرة الذاتية.

**أنواع الأقسام:**
1. **المعلومات الشخصية** (مطلوب)
   - الاسم الكامل
   - البريد الإلكتروني
   - رقم الهاتف
   - العنوان
   - الموقع الجغرافي
   - روابط التواصل (LinkedIn, GitHub, إلخ)

2. **الملخص المهني** (اختياري)
   - وصف مختصر (100-300 كلمة)

3. **الخبرة العملية** (متعدد)
   - اسم الشركة
   - المسمى الوظيفي
   - تاريخ البدء والنهاية
   - الوصف الوظيفي
   - الإنجازات

4. **التعليم** (متعدد)
   - اسم المؤسسة التعليمية
   - الدرجة العلمية
   - التخصص
   - تاريخ التخرج
   - المعدل التراكمي (اختياري)

5. **المهارات** (متعدد)
   - اسم المهارة
   - مستوى الإتقان (مبتدئ، متوسط، متقدم، خبير)
   - السنوات من الخبرة

6. **المشاريع** (متعدد)
   - اسم المشروع
   - الوصف
   - التقنيات المستخدمة
   - الرابط (اختياري)

7. **الشهادات** (متعدد)
   - اسم الشهادة
   - الجهة المانحة
   - تاريخ الحصول
   - رقم الشهادة (اختياري)

8. **اللغات** (متعدد)
   - اللغة
   - مستوى الإتقان (A1, A2, B1, B2, C1, C2)

**واجهة إدارة الأقسام:**
- إضافة قسم جديد (زر +)
- تعديل قسم موجود (زر تحرير)
- حذف قسم (زر حذف مع تأكيد)
- إعادة ترتيب الأقسام (سحب وإفلات)
- إظهار/إخفاء الأقسام

### FR-003: معاينة السيرة الذاتية
**الوصف:** المستخدم يمكنه معاينة السيرة الذاتية أثناء الإنشاء.

**المتطلبات:**
- معاينة مباشرة أثناء التحرير
- تبديل بين وضع التحرير والمعاينة
- معاينة متجاوبة (Desktop, Tablet, Mobile)
- خيارات التصدير:
  - PDF (عالية الجودة)
  - Word (.docx)
  - نص عادي (.txt)

### FR-004: إدارة القوالب
**الوصف:** النظام يوفر قوالب جاهزة للسير الذاتية.

**أنواع القوالب:**
1. **كلاسيكي** - تصميم تقليدي ومنظم
2. **حديث** - تصميم عصري مع صور
3. **مبدع** - تصميم إبداعي للمصممين
4. **تقني** - تصميم للمطورين والمهندسين
5. **أكاديمي** - تصميم للباحثين والأكاديميين

**خصائص كل قالب:**
- هيكل الأقسام المحدد مسبقاً
- تنسيقات CSS جاهزة
- أماكن محددة للصور
- خيارات تخصيص الألوان والخطوط

## 🔧 المتطلبات غير الوظيفية

### NFR-001: الأداء
- وقت تحميل صفحة إنشاء السيرة الذاتية < 2 ثانية
- وقت حفظ التعديلات < 1 ثانية
- دعم حتى 100 قسم في السيرة الواحدة
- معاينة مباشرة بدون تأخير ملحوظ

### NFR-002: الأمان
- التحقق من صلاحيات المستخدم قبل كل عملية
- تشفير البيانات الحساسة (البريد، الهاتف)
- منع الوصول إلى سير ذاتية لمستخدمين آخرين
- تسجيل جميع عمليات الإنشاء والتعديل

### NFR-003: سهولة الاستخدام
- واجهة بديهية لا تحتاج إلى تدريب
- إرشادات خطوة بخطوة
- رسائل خطأ واضحة
- دعم اللغة العربية بشكل كامل

### NFR-004: التوافقية
- متوافق مع جميع المتصفحات الحديثة
- متجاوب مع جميع أحجام الشاشات
- دعم وضع عدم الاتصال (حفظ محلي)
- دعم تصدير بصيغ متعددة

## 🎨 تصميم الواجهة

### مكونات Angular

#### 1. CvCreateComponent
```typescript
@Component({
  selector: 'app-cv-create',
  templateUrl: './cv-create.component.html',
  styleUrls: ['./cv-create.component.scss']
})
export class CvCreateComponent {
  // Properties
  cvForm: FormGroup;
  availableTemplates: CvTemplate[];
  selectedTemplate: CvTemplate;
  sections: CvSection[] = [];
  
  // Methods
  createNewCv(): void;
  selectTemplate(template: CvTemplate): void;
  addSection(sectionType: SectionType): void;
  removeSection(sectionId: string): void;
  reorderSections(oldIndex: number, newIndex: number): void;
  saveCv(): Observable<Cv>;
  previewCv(): void;
}
```

#### 2. CvSectionComponent
```typescript
@Component({
  selector: 'app-cv-section',
  templateUrl: './cv-section.component.html',
  styleUrls: ['./cv-section.component.scss']
})
export class CvSectionComponent {
  @Input() section: CvSection;
  @Input() sectionType: SectionType;
  @Output() sectionUpdated = new EventEmitter<CvSection>();
  @Output() sectionDeleted = new EventEmitter<string>();
  
  sectionForm: FormGroup;
  
  ngOnInit(): void {
    this.initializeForm();
  }
  
  initializeForm(): void;
  saveSection(): void;
  deleteSection(): void;
}
```

#### 3. CvPreviewComponent
```typescript
@Component({
  selector: 'app-cv-preview',
  templateUrl: './cv-preview.component.html',
  styleUrls: ['./cv-preview.component.scss']
})
export class CvPreviewComponent {
  @Input() cv: Cv;
  @Input() sections: CvSection[];
  
  exportAsPdf(): void;
  exportAsWord(): void;
  printCv(): void;
  shareCv(): void;
}
```

### Services

#### 1. CvService
```typescript
@Injectable({
  providedIn: 'root'
})
export class CvService {
  constructor(private http: HttpClient) {}
  
  createCv(cvData: CreateCvDto): Observable<Cv>;
  updateCv(cvId: string, cvData: UpdateCvDto): Observable<Cv>;
  getCv(cvId: string): Observable<Cv>;
  deleteCv(cvId: string): Observable<void>;
  getCvSections(cvId: string): Observable<CvSection[]>;
  addSection(cvId: string, sectionData: CreateSectionDto): Observable<CvSection>;
  updateSection(sectionId: string, sectionData: UpdateSectionDto): Observable<CvSection>;
  deleteSection(sectionId: string): Observable<void>;
  reorderSections(cvId: string, sectionOrder: string[]): Observable<void>;
}
```

#### 2. CvTemplateService
```typescript
@Injectable({
  providedIn: 'root'
})
export class CvTemplateService {
  getTemplates(): Observable<CvTemplate[]>;
  getTemplate(templateId: string): Observable<CvTemplate>;
  applyTemplate(cvId: string, templateId: string): Observable<Cv>;
}
```

#### 3. CvExportService
```typescript
@Injectable({
  providedIn: 'root'
})
export class CvExportService {
  exportToPdf(cv: Cv, sections: CvSection[]): Observable<Blob>;
  exportToWord(cv: Cv, sections: CvSection[]): Observable<Blob>;
  generateShareableLink(cvId: string): Observable<string>;
}
```

## 🗄️ نماذج البيانات (DTOs)

### CreateCvDto
```typescript
interface CreateCvDto {
  title: string;
  summary?: string;
  templateId?: string;
  languageCode?: string;
}
```

### UpdateCvDto
```typescript
interface UpdateCvDto {
  title?: string;
  summary?: string;
  status?: CvStatus;
}
```

### CvSectionDto
```typescript
interface CvSectionDto {
  id: string;
  cvId: string;
  sectionType: SectionType;
  title: string;
  content: any; // JSON object
  displayOrder: number;
  createdAt: Date;
}
```

### CreateSectionDto
```typescript
interface CreateSectionDto {
  sectionType: SectionType;
  title: string;
  content: any;
}
```

## 🔌 APIs

### 1. إنشاء سيرة ذاتية جديدة
```
POST /api/cvs
Content-Type: application/json

{
  "title": "سيرتي الذاتية",
  "summary": "ملخص عن خبراتي...",
  "templateId": "optional-template-id",
  "languageCode": "ar"
}

Response: 201 Created
{
  "id": "cv-id",
  "title": "سيرتي الذاتية",
  "status": "Draft",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### 2. إضافة قسم جديد
```
POST /api/cvs/{cvId}/sections
Content-Type: application/json

{
  "sectionType": "Experience",
  "title": "الخبرة العملية",
  "content": {
    "company": "شركة التقنية",
    "position": "مطور برمجيات",
    "startDate": "2020-01-01",
    "endDate": "2023-12-31",
    "description": "تطوير تطبيقات الويب..."
  }
}

Response: 201 Created
{
  "id": "section-id",
  "title": "الخبرة العملية",
  "sectionType": "Experience",
  "displayOrder": 1
}
```

### 3. تصدير السيرة الذاتية
```
GET /api/cvs/{cvId}/export/pdf
Accept: application/pdf

Response: 200 OK
Content-Type: application/pdf
```

## 🧪 حالات الاختبار

### اختبارات الوحدة
1. `CvService.createCv()` يجب أن ترسل طلب POST صحيح
2. `CvSectionComponent` يجب أن يعرض البيانات بشكل صحيح
3. `CvExportService.exportToPdf()` يجب أن تولد PDF

### اختبارات التكامل
1. إنشاء سيرة ذاتية كاملة من البداية
2. إضافة وتعديل وحذف الأقسام
3. تصدير السيرة الذاتية بصيغة PDF

### اختبارات الأداء
1. تحميل صفحة الإنشاء مع 50 قسم
2. وقت حفظ سيرة ذاتية كبيرة
3. وقت تصدير PDF لسيرة ذاتية معقدة

## 📊 متطلبات قاعدة البيانات

### Indexes
```sql
-- للبحث السريع
CREATE INDEX idx_cvs_user_id ON Cvs(UserId);
CREATE INDEX idx_cvs_status ON Cvs(Status);
CREATE INDEX idx_cv_sections_cv_id ON CvSections(CvId);
CREATE INDEX idx_cv_sections_order ON CvSections(CvId, DisplayOrder);
```

### Constraints
```sql
-- ضمان أن كل مستخدم له عنوان فريد للسيرة الذاتية
ALTER TABLE Cvs ADD CONSTRAINT uq_cv_user_title UNIQUE (UserId, Title);

-- ضمان ترتيب الأقسام فريد لكل سيرة ذاتية
ALTER TABLE CvSections ADD CONSTRAINT uq_section_order UNIQUE (CvId, DisplayOrder);
```

## 📝 ملاحظات التنفيذ

1. **الحفظ التلقائي:** استخدام debounce لتجنب طلبات كثيرة
2. **المعاينة المباشرة:** استخدام Web Workers للمعالجة الثقيلة
3. **التصدير:** استخدام مكتبات مثل jsPDF و html-docx-js
4. **التنسيق:** استخدام CSS Grid/Flexbox للتصميم المتجاوب
5. **التحقق:** استخدام Reactive Forms مع validators مخصصة
6. **الأداء:** استخدام Virtual Scrolling للقوائم الطويلة
7. **التخزين:** استخدام IndexedDB للحفظ المحلي