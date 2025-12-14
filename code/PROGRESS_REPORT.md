# Global Progress Report

## Features

### FEAT-CV-CREATION
- Backend domain model for CVs created (`CV` entity, DbContext configuration).
- Application and Application.Contracts layers initialized for CVs (DTOs, ICvAppService, AutoMapper profile).
- CRUD application service for CVs implemented (no export yet).
- HTTP API CvController added under `code/backend/src/Http/API/Controllers` for CRUD endpoints.

## Notes
- Next steps for FEAT-CV-CREATION: implement export endpoints and logic, then start Angular frontend (creation form & preview).
