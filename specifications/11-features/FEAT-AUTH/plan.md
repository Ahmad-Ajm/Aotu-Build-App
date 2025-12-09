# الخطة التقنية: نظام المصادقة والأمان (FEAT-AUTH)

## نظرة عامة معمارية

### الهيكل العام
سيتم بناء نظام المصادقة كجزء أساسي من تطبيق الويب، مع فصل واضح بين:
1. **واجهة المستخدم (Frontend)**: Angular application
2. **الخلفية (Backend)**: .NET 8 Web API مع ABP Framework
3. **قاعدة البيانات**: PostgreSQL
4. **خدمات الدعم**: Email service, Token service

### المكونات الرئيسية

#### 1. Authentication Module (وحدة المصادقة)
- **Login Controller**: معالجة طلبات تسجيل الدخول
- **Logout Controller**: معالجة طلبات تسجيل الخروج
- **Password Recovery Controller**: إدارة استعادة كلمات المرور
- **Token Service**: توليد وتحقق من الرموز (JWT)

#### 2. User Management Module (وحدة إدارة المستخدمين)
- **User Controller**: عمليات CRUD على المستخدمين
- **User Service**: منطق الأعمال لإدارة المستخدمين
- **User Repository**: وصول إلى بيانات المستخدمين

#### 3. Authorization Module (وحدة التفويض)
- **Role Controller**: إدارة الأدوار
- **Permission Controller**: إدارة الصلاحيات
- **Authorization Service**: التحقق من الصلاحيات

#### 4. Session Management Module (وحدة إدارة الجلسات)
- **Session Service**: تتبع وإدارة الجلسات
- **Audit Log Service**: تسجيل أحداث الأمان

## نموذج البيانات (Data Model)

### الجداول الرئيسية

#### 1. Users (المستخدمين)
```sql
CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FullName VARCHAR(255) NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    LastLogin TIMESTAMP,
    FailedLoginAttempts INT DEFAULT 0,
    LockoutEnd TIMESTAMP,
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW()
);
```

#### 2. Roles (الأدوار)
```sql
CREATE TABLE Roles (
    Id UUID PRIMARY KEY,
    Name VARCHAR(100) UNIQUE NOT NULL,
    Description TEXT,
    IsSystemRole BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT NOW()
);
```

#### 3. Permissions (الصلاحيات)
```sql
CREATE TABLE Permissions (
    Id UUID PRIMARY KEY,
    Name VARCHAR(100) UNIQUE NOT NULL,
    Description TEXT,
    Category VARCHAR(100),
    CreatedAt TIMESTAMP DEFAULT NOW()
);
```

#### 4. UserRoles (علاقة المستخدمين بالأدوار)
```sql
CREATE TABLE UserRoles (
    UserId UUID REFERENCES Users(Id) ON DELETE CASCADE,
    RoleId UUID REFERENCES Roles(Id) ON DELETE CASCADE,
    AssignedAt TIMESTAMP DEFAULT NOW(),
    PRIMARY KEY (UserId, RoleId)
);
```

#### 5. RolePermissions (علاقة الأدوار بالصلاحيات)
```sql
CREATE TABLE RolePermissions (
    RoleId UUID REFERENCES Roles(Id) ON DELETE CASCADE,
    PermissionId UUID REFERENCES Permissions(Id) ON DELETE CASCADE,
    GrantedAt TIMESTAMP DEFAULT NOW(),
    PRIMARY KEY (RoleId, PermissionId)
);
```

#### 6. PasswordResetTokens (رموز استعادة كلمة المرور)
```sql
CREATE TABLE PasswordResetTokens (
    Id UUID PRIMARY KEY,
    UserId UUID REFERENCES Users(Id) ON DELETE CASCADE,
    Token VARCHAR(255) UNIQUE NOT NULL,
    ExpiresAt TIMESTAMP NOT NULL,
    IsUsed BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT NOW()
);
```

#### 7. AuditLogs (سجلات التدقيق)
```sql
CREATE TABLE AuditLogs (
    Id UUID PRIMARY KEY,
    UserId UUID REFERENCES Users(Id),
    Action VARCHAR(100) NOT NULL,
    EntityType VARCHAR(100),
    EntityId VARCHAR(255),
    OldValues JSONB,
    NewValues JSONB,
    IpAddress VARCHAR(45),
    UserAgent TEXT,
    CreatedAt TIMESTAMP DEFAULT NOW()
);
```

## واجهات برمجة التطبيقات (APIs)

### Authentication APIs

#### POST /api/auth/login
**الوصف**: تسجيل دخول المستخدم
**المدخلات**:
```json
{
    "email": "user@example.com",
    "password": "password123"
}
```
**المخرجات**:
```json
{
    "accessToken": "jwt_token_here",
    "refreshToken": "refresh_token_here",
    "expiresIn": 3600,
    "user": {
        "id": "uuid",
        "email": "user@example.com",
        "fullName": "User Name",
        "roles": ["Admin", "User"]
    }
}
```

#### POST /api/auth/logout
**الوصف**: تسجيل خروج المستخدم
**المدخلات**: (Bearer Token في Header)
**المخرجات**: 
```json
{
    "success": true,
    "message": "Logged out successfully"
}
```

#### POST /api/auth/refresh-token
**الوصف**: تجديد الرمز المميز
**المدخلات**:
```json
{
    "refreshToken": "refresh_token_here"
}
```

#### POST /api/auth/forgot-password
**الوصف**: طلب استعادة كلمة المرور
**المدخلات**:
```json
{
    "email": "user@example.com"
}
```

#### POST /api/auth/reset-password
**الوصف**: تعيين كلمة مرور جديدة
**المدخلات**:
```json
{
    "token": "reset_token_here",
    "newPassword": "new_password_123"
}
```

### User Management APIs

#### GET /api/users
**الوصف**: الحصول على قائمة المستخدمين
**الاستعلامات**: page, pageSize, search, isActive
**الصلاحية المطلوبة**: Users.View

#### GET /api/users/{id}
**الوصف**: الحصول على مستخدم محدد
**الصلاحية المطلوبة**: Users.View

#### POST /api/users
**الوصف**: إنشاء مستخدم جديد
**المدخلات**:
```json
{
    "email": "new@example.com",
    "fullName": "New User",
    "password": "initial_password",
    "roleIds": ["role_uuid_1", "role_uuid_2"]
}
```
**الصلاحية المطلوبة**: Users.Create

#### PUT /api/users/{id}
**الوصف**: تحديث مستخدم
**الصلاحية المطلوبة**: Users.Edit

#### DELETE /api/users/{id}
**الوصف**: حذف مستخدم (تعطيل)
**الصلاحية المطلوبة**: Users.Delete

### Role Management APIs

#### GET /api/roles
**الوصف**: الحصول على قائمة الأدوار
**الصلاحية المطلوبة**: Roles.View

#### POST /api/roles
**الوصف**: إنشاء دور جديد
**الصلاحية المطلوبة**: Roles.Create

#### PUT /api/roles/{id}
**الوصف**: تحديث دور
**الصلاحية المطلوبة**: Roles.Edit

#### POST /api/roles/{id}/permissions
**الوصف**: تعيين صلاحيات للدور
**الصلاحية المطلوبة**: Roles.ManagePermissions

## تصميم واجهة المستخدم (UI Design)

### الصفحات الرئيسية

#### 1. صفحة تسجيل الدخول (Login Page)
- **العناصر**:
  - حقل البريد الإلكتروني
  - حقل كلمة المرور
  - زر "تسجيل الدخول"
  - رابط "نسيت كلمة المرور"
  - رسائل الخطأ والتحقق

#### 2. صفحة استعادة كلمة المرور (Forgot Password)
- **العناصر**:
  - حقل البريد الإلكتروني
  - زر "إرسال رابط الاستعادة"
  - رسالة تأكيد بعد الإرسال
  - رابط للعودة لتسجيل الدخول

#### 3. صفحة تعيين كلمة مرور جديدة (Reset Password)
- **العناصر**:
  - حقل كلمة المرور الجديدة
  - حقل تأكيد كلمة المرور
  - زر "تعيين كلمة المرور"
  - التحقق من قوة كلمة المرور

#### 4. صفحة إدارة المستخدمين (Users Management)
- **العناصر**:
  - جدول بقائمة المستخدمين
  - زر إضافة مستخدم جديد
  - أزرار التعديل والحذف
  - فلترات البحث
  - ترقيم الصفحات

#### 5. صفحة إدارة الأدوار (Roles Management)
- **العناصر**:
  - قائمة بالأدوار
  - زر إضافة دور جديد
  - أزرار إدارة الصلاحيات
  - عرض الصلاحيات الممنوحة لكل دور

### المكونات (Components)

#### 1. Login Form Component
- نموذج تسجيل الدخول مع التحقق
- معالجة الأخطاء
- توجيه بعد التسجيل الناجح

#### 2. User List Component
- عرض جدول المستخدمين
- دعم البحث والترشيح
- تفعيل/تعطيل المستخدمين

#### 3. User Form Component
- نموذج إضافة/تعديل المستخدم
- اختيار الأدوار
- التحقق من البيانات

#### 4. Role Form Component
- نموذج إدارة الأدوار
- اختيار الصلاحيات
- حفظ وتحديث الأدوار

## الاعتبارات الأمنية

### 1. حماية كلمات المرور
- استخدام خوارزمية PBKDF2 أو bcrypt للتجزئة
- إضافة Salt عشوائي لكل مستخدم
- الحد الأدنى لطول كلمة المرور: 8 أحرف
- متطلبات التعقيد: أحرف كبيرة وصغيرة وأرقام ورموز

### 2. حماية الجلسات
- استخدام JWT مع توقيع قوي
- صلاحية محدودة للرموز (ساعة واحدة للوصول، أسبوع للتجديد)
- تخزين آمن للرموز على العميل
- القدرة على إلغاء الرموز المسروقة

### 3. الحماية من الهجمات
- تحديد محاولات تسجيل الدخول الفاشلة (5 محاولات)
- قفل الحساب المؤقت بعد المحاولات الفاشلة
- الحماية من هجمات CSRF
- التحقق من صحة المدخلات

### 4. سجلات الأمان
- تسجيل جميع محاولات تسجيل الدخول
- تسجيل تغييرات الصلاحيات
- تسجيل عمليات إدارة المستخدمين
- الاحتفاظ بالسجلات لمدة 90 يومًا على الأقل

## التبعيات والمخاطر

### التبعيات
1. **ABP Framework**: يجب تثبيت وتكوين ABP Framework
2. **PostgreSQL**: يجب إعداد قاعدة البيانات
3. **Email Service**: خدمة لإرسال رسائل استعادة كلمة المرور
4. **JWT Library**: مكتبة لتوليد وتحقق من JWT tokens

### المخاطر
1. **مخاطر الأمان**: ثغرات في تنفيذ المصادقة
2. **مخاطر الأداء**: تأثير نظام الصلاحيات على الأداء
3. **مخاطر التوافق**: مشاكل في تكامل ABP مع المكونات الأخرى
4. **مخاطر البيانات**: فقدان بيانات المستخدمين

### استراتيجيات التخفيف
1. **اختبارات الأمان**: اختبارات اختراق شاملة
2. **التخزين المؤقت**: تخزين مؤقت للصلاحيات
3. **النسخ الاحتياطي**: نسخ احتياطي منتظم للبيانات
4. **المراقبة**: مراقبة محاولات تسجيل الدخول الفاشلة

## جدول التنفيذ المقترح

### الأسبوع 1: الإعداد الأساسي
- إعداد مشروع .NET 8 مع ABP
- تكوين قاعدة البيانات PostgreSQL
- إنشاء نماذج البيانات الأساسية
- إعداد نظام الهجرة (Migrations)

### الأسبوع 2: نظام المصادقة الأساسي
- تنفيذ تسجيل الدخول والخروج
- تنفيذ JWT token service
- إنشاء واجهات برمجة التطبيقات للمصادقة
- اختبارات الوحدة للمصادقة

### الأسبوع 3: إدارة المستخدمين
- تنفيذ CRUD للمستخدمين
- نظام تفعيل/تعطيل المستخدمين
- واجهة إدارة المستخدمين في Angular
- اختبارات التكامل

### الأسبوع 4: نظام الصلاحيات والتحسينات
- تنفيذ نظام RBAC
- واجهة إدارة الأدوار والصلاحيات
- استعادة كلمة المرور
- سجلات التدقيق الأمني
- الاختبارات النهائية والتحسينات

## الافتراضات والقرارات التقنية

### الافتراضات:
1. سيكون عدد المستخدمين أقل من 500 في المرحلة الأولى
2. النظام سيعمل في بيئة آمنة (HTTPS)
3. سيكون هناك مسؤول واحد على الأقل لإدارة النظام
4. المستخدمون سيتعاملون مع النظام من متصفحات حديثة

### القرارات التقنية:
1. **Backend Framework**: .NET 8 مع ABP Framework
2. **Frontend Framework**: Angular 15+
3. **Database**: PostgreSQL 14+
4. **Authentication**: JWT مع Refresh Tokens
5. **Password Hashing**: PBKDF2 مع HMAC-SHA256
6. **API Documentation**: Swagger/OpenAPI
7. **Testing**: xUnit للـ .NET، Jasmine/Karma للـ Angular

### القرارات المعمارية:
1. **Clean Architecture**: فصل الطبقات بوضوح
2. **Repository Pattern**: لفصل منطق الوصول إلى البيانات
3. **Dependency Injection**: لإدارة التبعيات
4. **Middleware Pipeline**: للمعالجة المركزية (التحقق من الصلاحيات، السجلات)
5. **Response Wrapping**: لتوحيد استجابات واجهات برمجة التطبيقات