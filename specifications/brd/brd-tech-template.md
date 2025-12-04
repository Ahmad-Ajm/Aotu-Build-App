# Technical BRD – [اسم النظام]

> ملاحظة للذكاء:  
> - هذا القالب موجه لشركة تقنية / فريق هندسي يهتم بالتفاصيل.  
> - استخدم العربية للشرح العام، مع استخدام المصطلحات والأدوات التقنية بالإنجليزية (Frameworks, Libraries, Services, Tools).  
> - اربط كل قسم قدر الإمكان بخريطة الميزات `.info/17-project/features-map.md`.  
> - لا تضف محتوى `.info/prd/مقطع ثابت.md` هنا؛ سيُضاف تلقائيًا في نهاية الوثيقة.

---

## 1. Overview & Context

[وصف تقني مختصر للنظام، البيئة العامة، والـ Domain.]

- نوع النظام: (مثلاً: B2C Web Platform / B2B SaaS / Mobile App + Backend).
- المستفيدون الرئيسيون (Users / Roles).
- ملخص الـ Business Domain.

---

## 2. Objectives & Key Use Cases

[أهداف تقنية/وظيفية، مع أهم الـ Use Cases.]

- تحسين الأداء / الاستقرارية / Observability.
- دعم سيناريوهات معينة (Use Cases) مثل:
  - UC-01: User Registration & Authentication
  - UC-02: Project Publishing
  - UC-03: Loyalty Points Redemption
  - ...

---

## 3. Architecture Overview

[صورة عالية المستوى للمعمارية المقترحة.]

- Style: `Layered Architecture`, `Clean Architecture`, `Microservices`, `Modular Monolith`, ...
- Components:
  - API Layer: `ASP.NET Core Web API` (مثال).
  - Application Layer: استخدام `CQRS`, `MediatR`, `Domain Events` إن وجدت.
  - Data Access: `EF Core` / `Dapper` / `Repository Pattern`.
- Integration Points:
  - External Services (Payment Gateway, SMS/Email Providers, Identity Provider, ...).

يمكن إضافة Diagrams خارجية (C4 Model) والإشارة إليها هنا.

---

## 4. Tech Stack & Tools

[قائمة مفصلة بالتقنيات والأدوات المستخدمة.]

- Backend:
  - `ASP.NET Core` [Version]
  - `EF Core` [Version]
  - `AutoMapper`, `FluentValidation`, ...
- Frontend:
  - `Angular` [Version] / `React` / `Next.js`
  - UI Library: `LeptonX`, `Bootstrap`, `TailwindCSS`, ...
- Database & Storage:
  - `PostgreSQL` / `SQL Server`
  - Caching: `Redis`
  - File Storage: `Azure Blob Storage` / `S3` / Local
- Messaging & Integration:
  - `RabbitMQ` / `Kafka` (إن وجد).
- DevOps:
  - `Azure DevOps` / `GitHub Actions`
  - CI/CD Pipeline
- Observability:
  - `Serilog`, `Seq`, `Application Insights`, `Prometheus + Grafana`...

(حدّث القائمة حسب مشروعك.)

---

## 5. Domain Model & Data Design

### 5.1 Domain Model

[ملخص لأهم الـ Aggregates / Entities في الدومين.]

- Entities رئيسية (مثال): `User`, `Project`, `Subscription`, `LoyaltyAccount`, ...
- وصف مختصر لكل Entity وعلاقته بالميزات في `features-map.md`.

### 5.2 Database Schema & ERD

[وصف عالي المستوى للـ Schema، مع ربط بـ `.info/06-data/*`.]

- جداول أساسية:
  - `Users`, `Projects`, `Subscriptions`, `Transactions`, ...
- ملاحظات على الـ Indexes, Constraints, Partitioning (إن وجدت).

---

## 6. APIs Design

[تفصيل للـ APIs الرئيسية مع نمط الـ Endpoint.]

- نمط التسمية: `/api/v1/...`
- أمثلة:
  - `POST /api/v1/auth/register`
  - `POST /api/v1/projects`
  - `GET /api/v1/projects/{id}`
  - `POST /api/v1/loyalty/earn`
  - `POST /api/v1/loyalty/redeem`
- اذكر:
  - Request/Response models (بإيجاز).
  - Authentication & Authorization requirements.
  - Validation / Error Handling approach.

---

## 7. Non-Functional Requirements (Detailed)

[تفصيل تقني لمتطلبات الأداء، الأمان، التوافر، القابلية للتوسع، ...].

- Performance:
  - Target latency, throughput, load profile.
  - Caching strategy (`Redis`, in-memory cache, ...).
- Security:
  - `HTTPS` everywhere.
  - `JWT` / `OpenID Connect` / `OAuth2`.
  - OWASP considerations (XSS, CSRF, SQL Injection, ...).
- Scalability:
  - Horizontal scaling for API layer.
  - Database scaling strategy (Read Replicas, Sharding, ... إذا لزم).
- Availability:
  - Target SLA (مثلاً: 99.5%).
  - Failover / DR strategy.

---

## 8. DevOps & CI/CD

[خطة الـ Pipeline كما سيتم تنفيذها.]

- Source Control & Branching Strategy (GitFlow, Trunk-Based, ...).
- CI:
  - Build, Unit Tests, Code Quality Checks.
- CD:
  - Environments: `Dev`, `Staging`, `Production`.
  - Deployment Strategy: `Blue-Green`, `Rolling`, ...
- Infrastructure as Code:
  - `Terraform` / `Bicep` / `ARM` / `Pulumi` (إن وجدت).

---

## 9. Testing Strategy

[تلخيص لخطة الاختبار التقنية، مرتبطة بـ `.info/12-testing/*`.]

- Unit Tests
- Integration Tests
- End-to-End Tests
- Performance & Load Testing
- Security Testing

---

## 10. Risks & Open Questions

[قائمة بالمخاطر التقنية والأسئلة المفتوحة.]

- Risk 1: ...
- Risk 2: ...
- Open Question 1: ...


