# Progress Report - تسجيل المستخدمين (FEAT-USER-REGISTRATION)

## Feature Status: In Progress (Attempt 2)

## CI Status
- [ ] Attempt 1: CI Failed (backend `dotnet restore` could not find a project/solution in `code/backend/src/Http/API`).
- [ ] Attempt 2: CI Not Run Yet (current attempt).

## CI Fix Attempts Summary
1. Created placeholder solution file `code/backend/CVSystem.sln` so that we can later point CI to a real solution and group backend projects.
2. Next runs should update CI or solution structure so `dotnet restore` runs from a directory that actually contains a `.sln` or project, or adjust the working directory to `code/backend`.

---

## Tasks Overview (from tasks.md)

1. Task 1: إعداد بيئة التطوير (Backend)
   - Status: In Progress (creating real .NET solution and projects under `code/backend` that match SpecKit architecture: Application, Domain, EntityFrameworkCore, Http/API).
2. Task 2: تصميم قاعدة البيانات (Backend)
   - Status: Not Started (will be implemented via EF Core entities and migrations after base projects exist).
3. Task 3: تطوير خدمة المستخدمين (Backend)
   - Status: Not Started.
4. Task 4: تطوير خدمة المصادقة (Backend)
   - Status: Not Started.
5. Task 5: تطوير المتحكمات (Backend)
   - Status: Not Started.
6. Task 6: تطوير Frontend - المكونات
   - Status: Not Started.
7. Task 7: تطوير Frontend - الخدمات
   - Status: Not Started.
8. Task 8: تطوير Frontend - التوجيه
   - Status: Not Started.
9. Task 9: التكامل والاختبار
   - Status: Not Started.
10. Task 10: التوثيق والمراجعة
    - Status: Not Started.

---

## Current Attempt (Attempt 2) Plan

### Backend
- Create a minimal but compilable backend solution under `code/backend` that follows ABP-like layering:
  - `CVSystem.Domain`
  - `CVSystem.Application`
  - `CVSystem.EntityFrameworkCore`
  - `CVSystem.HttpApi`
- Implement basic user + user profile entities and DbContext according to SpecKit table design.
- Implement minimal registration & login services and Http API endpoints matching the specified contracts (without full ABP dependency, but compatible with .NET 8, EF Core, and JWT where practical).

### Frontend
- Not in scope for this attempt; will be started after backend compiles and CI for backend passes.

### Definition of Done for Attempt 2
- `dotnet restore` and `dotnet build` succeed for the backend solution in CI.
- Minimal API surface for:
  - `POST /api/auth/register`
  - `POST /api/auth/login`
- Global and feature-level progress reports updated.
