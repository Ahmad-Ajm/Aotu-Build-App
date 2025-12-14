# خريطة الكود – Code Map

> هذا الملف يصف جذور الكود الفعلية في المشروع الحالي (CV System) ويربطها بالميزات والوثائق في مجلد `specifications`.  
> يمكن تحديثه يدويًا مع تطور المعمارية (مثل إضافة خدمات خارجية أو Frontends إضافية).

## جذور الكود (Code Roots)

| RootName      | نوع الجذر (Backend/Frontend/Service/Library) | المسار (Path) | اللغات/التقنيات الرئيسية              | ملاحظات |
|---------------|----------------------------------------------|---------------|----------------------------------------|---------|
| Backend-CV    | Backend                                     | code/backend  | .NET 8, ABP, Entity Framework Core     | يحتوي على الحل `CVSystem.sln` مع المشاريع Domain/Application/EFCore/Http.API/HttpApi.Host، ويعرّض APIs مثل `/api/app/cv` و`/api/auth` و`/api/account`. |
| Frontend-CV   | Frontend                                    | code/frontend | Angular 17, TypeScript, Angular Material | واجهة المستخدم الرئيسية، تشمل وحدات `CVModule` لإنشاء/إدارة السير الذاتية و`AuthModule` للتسجيل وتسجيل الدخول والملف الشخصي. |

## نطاقات فحص الكود (Code Scan Scopes)

> يمكن استخدام هذه النطاقات عند تحليل الكود أو ربطه بميزات SpecKit (مثل FEAT-CV-CREATION و FEAT-USER-REGISTRATION).

| Scope Name             | RootName    | المسار (Path)         | الحالة     | ملاحظات |
|------------------------|------------|------------------------|------------|---------|
| Backend-CV-Core        | Backend-CV | code/backend/src       | Stable     | يحتوي الطبقات الأساسية (Domain, Application, EFCore, Http.API) المرتبطة بإنشاء السير الذاتية وتسجيل المستخدمين. |
| Backend-CV-Host        | Backend-CV | code/backend/HttpApi.Host | Stable  | مشروع الاستضافة (ASP.NET Core) الذي يشغّل الـ APIs على `http://localhost:5134`. |
| Frontend-CV-App        | Frontend-CV| code/frontend/src/app  | Stable     | يحتوي وحدات الميزات `features/cv` و`features/auth` وربطها بالـ Routing وخدمات التكامل مع الـ Backend. |
| Frontend-CV-RootConfig | Frontend-CV| code/frontend          | Stable     | ملفات إعداد Angular (`angular.json`, `tsconfig`, `proxy.conf.json`, `main.ts`, `index.html`) اللازمة لتشغيل الواجهة على `http://localhost:4400` أو `4401`. |

## ملاحظات

- يمكن إضافة جذور جديدة (مثل Mobile App أو خدمات Background Jobs) في حال توسّع المشروع مستقبلاً.
- يوصى بمزامنة هذه الخريطة مع ملفات المواصفات ذات العلاقة:
  - ميزات إنشاء السيرة الذاتية: `psec-kit-file/FEAT-CV-CREATION/*`
  - ميزات تسجيل المستخدمين: `psec-kit-file/FEAT-USER-REGISTRATION/*`
- يمكن للأدوار الآلية (إن وُجدت) الاعتماد على هذا الملف لفهم بنية الكود وربطها بالميزات وSpecKit.
