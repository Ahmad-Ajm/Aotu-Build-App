# نظام إنشاء السيرة الذاتية (CV Creation System)

## نظرة عامة
نظام متكامل لإنشاء وإدارة السير الذاتية باستخدام .NET 8 + ABP Framework في Backend و Angular 17 في Frontend.

## بنية المشروع

### Backend (.NET 8 + ABP Framework)
```
code/
├── backend/
│   ├── src/
│   │   ├── Domain/
│   │   │   ├── Entities/
│   │   │   │   ├── CV.cs
│   │   │   │   ├── Education.cs
│   │   │   │   ├── Experience.cs
│   │   │   │   ├── Skill.cs
│   │   │   │   └── ContactInfo.cs
│   │   │   ├── Shared/
│   │   │   └── Services/
│   │   ├── Application/
│   │   │   ├── Contracts/
│   │   │   ├── Services/
│   │   │   └── DTOs/
│   │   ├── Infrastructure/
│   │   │   ├── EntityFrameworkCore/
│   │   │   └── Repositories/
│   │   └── Web/
│   │       ├── Controllers/
│   │       └── Middleware/
│   └── tests/
```

### Frontend (Angular 17)
```
code/
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── cv/
│   │   │   │   ├── cv-creation/
│   │   │   │   ├── cv-editor/
│   │   │   │   ├── cv-preview/
│   │   │   │   ├── cv-list/
│   │   │   │   └── services/
│   │   │   ├── shared/
│   │   │   └── core/
│   │   ├── assets/
│   │   └── environments/
│   └── tests/
```

### قاعدة البيانات (PostgreSQL/SQL Server)
```
code/
├── database/
│   ├── migrations/
│   ├── scripts/
│   └── seed-data/
```

## الميزات المطلوبة

### FEAT-CV-CREATION (إنشاء السيرة الذاتية)
1. إنشاء سيرة ذاتية جديدة
2. تحرير المعلومات الشخصية
3. إدارة الخبرات العملية
4. إدارة التعليم والمؤهلات
5. إدارة المهارات
6. معاينة السيرة الذاتية
7. تصدير إلى PDF/DOCX
8. إدارة القوالب

### FEAT-USER-REGISTRATION (تسجيل المستخدمين)
1. تسجيل مستخدم جديد
2. تسجيل الدخول
3. إدارة الملف الشخصي
4. استعادة كلمة المرور

## التقنيات المستخدمة

### Backend
- .NET 8
- ABP Framework
- Entity Framework Core
- PostgreSQL / SQL Server
- Redis (لتخزين الجلسات)
- AutoMapper
- Swagger/OpenAPI

### Frontend
- Angular 17
- TypeScript
- RxJS
- Bootstrap 5 / Angular Material
- Chart.js (للإحصائيات)
- html2pdf.js (للتصدير)

### DevOps
- Docker
- Docker Compose
- GitHub Actions
- Nginx

## خطوات البدء

### 1. إعداد بيئة التطوير
```bash
# Clone المشروع
git clone <repository-url>

# Backend
cd code/backend
dotnet restore
dotnet build

# Frontend
cd code/frontend
npm install
npm start
```

### 2. إعداد قاعدة البيانات
```bash
# PostgreSQL
docker run --name cv-db -e POSTGRES_PASSWORD=password -p 5432:5432 -d postgres

# أو SQL Server
docker run --name cv-sql -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourPassword123' -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

### 3. تشغيل المشروع
```bash
# باستخدام Docker Compose
docker-compose up -d
```

## التوثيق
- [مواصفات الميزات](specifications/)
- [واجهة برمجة التطبيقات](docs/api/)
- [دليل المطور](docs/developer/)
- [دليل المستخدم](docs/user/)

## المساهمة
1. Fork المشروع
2. إنشاء فرع للميزة
3. Commit التغييرات
4. Push إلى الفرع
5. إنشاء Pull Request

## الترخيص
هذا المشروع مرخص تحت رخصة MIT.