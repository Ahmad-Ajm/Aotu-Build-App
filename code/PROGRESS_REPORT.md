# Project Progress Report

- Current Feature: FEAT-USER-REGISTRATION
- Status: In Progress
- Last CI Result: Failed (attempt 2 for FEAT-USER-REGISTRATION)

## Notes
- Fixed backend build issues related to AccountController and CVController:
  - Refactored AccountController to use ASP.NET Core MVC (ControllerBase) and IUserService with DTOs in CVSystem.Application.DTOs.
  - Refactored CVController to use ASP.NET Core MVC (ControllerBase) and ICVService with CV-related DTOs.
- Next steps:
  - Re-run CI for FEAT-USER-REGISTRATION to verify backend build passes.
  - Then review and align AuthController/AuthService/UserService with registration/login specifications.
