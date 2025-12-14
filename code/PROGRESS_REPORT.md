# Global Progress Report

## Features

### FEAT-CV-CREATION
- Backend domain model for CVs created (`CV` entity, DbContext configuration).
- Application and Application.Contracts layers initialized for CVs (DTOs, ICvAppService, AutoMapper profile).
- CRUD application service for CVs implemented (no export yet).
- HTTP API `CvController` added under `code/backend/src/Http/API/Controllers` for CRUD endpoints.

### FEAT-USER-REGISTRATION
- Backend user entities and DbContext already existed (`User`, `UserProfile`, `CVSystemDbContext` sets and configuration).
- New auth DTOs added for register/login responses and requests.
- `IUserAppService` contract created with `RegisterAsync` and `LoginAsync`.
- `UserAppService` implemented with basic registration/login logic (password hashing via SHA256, uniqueness check on email, update of `LastLoginDate`).
- `AuthController` added exposing `POST /api/auth/register` and `POST /api/auth/login`.
- Application.Contracts project aligned to net8.0 + ABP, Application module ensured to configure AutoMapper.

## Notes
- Next steps for FEAT-CV-CREATION: implement export endpoints and listing, then start Angular frontend (creation form & preview).
- Next steps for FEAT-USER-REGISTRATION: harden security (BCrypt, lockout after 5 failed attempts, proper JWT generation) and add profile endpoints, then build Angular registration/login UI.
