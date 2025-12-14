# Project Progress Report

- Current Feature: FEAT-USER-REGISTRATION
- Status: In Progress
- Last CI Result: Failed (attempt 1 for FEAT-USER-REGISTRATION)
- Notes:
  - Fixed backend build errors by refactoring AccountController to remove ABP (Volo.*) dependencies and use ASP.NET Core MVC ControllerBase with IUserService.
  - Updated CVController to include proper ASP.NET Core and DTO namespaces and aligned method signatures with existing ICVService and DTO types (CVDto, CreateCVDto, PublicCVDto).
  - Next: Re-run CI for FEAT-USER-REGISTRATION and continue implementing remaining user registration and authentication behaviors according to SpecKit.
