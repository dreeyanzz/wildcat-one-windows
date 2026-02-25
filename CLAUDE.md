# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Wildcat One** — a student-made Windows Forms desktop app (.NET 10.0, C#) that wraps CIT-U's student portal with a faster, more optimized UI. It talks to CIT-U's Azure-hosted backend APIs and encrypts all payloads with AES-CBC before sending them.

## Build & Run Commands

```bash
# Build
dotnet build

# Run
dotnet run

# Clean build artifacts
dotnet clean
```

Solution file: `wildcat-one-windows.slnx` (modern lightweight format). No external NuGet dependencies — uses only built-in .NET/WinForms libraries.

## Architecture

### Startup Flow
`Program.cs` → shows `NoticeForm` (disclaimer dialog, must be acknowledged) → `LoginForm` → on successful login, hides `LoginForm` and shows `Form1` (main dashboard); closing `Form1` closes `LoginForm`.

### Forms
- **`NoticeForm.cs`** — Disclaimer modal built entirely in code (no Designer file). Constructed via `InitializeNoticeForm()`.
- **`LoginForm.cs` / `LoginForm.Designer.cs`** — Handles login and forgot-password flows. Toggles between modes by showing/hiding fields and repositioning elements via `RepositionFormElements()`.
- **`Form1.cs` / `Form1.Designer.cs`** — Main app shell with a sidebar nav. Each page (Dashboard, Schedule, Grades, Professors, Course Offerings, Change Password) has its own state fields (e.g. `_schedulePageLoaded`, `_gradesPageLoaded`) and lazy-loads data on first visit. `Form1.Designer.cs` is auto-generated — do not hand-edit.

### Services (`Services/`)
- **`ApiService.cs`** — Central HTTP client. All requests go through `ApiService.CallAsync()`. Automatically encrypts request payloads via `CryptoHelper.EncryptPayload()` and decrypts `text/plain` responses via `CryptoHelper.DecryptPayload()`. Attaches HMAC signature headers (`X-HMAC-Nonce`, `X-HMAC-Salt`, `X-HMAC-Signature`) and Bearer token on every request. On HTTP 401 (non-login), calls `SessionManager.Instance.Reset()`.
- **`AuthService.cs`** — Login and forgot-password. On login success, fetches student info, academic years, and terms into `SessionManager`, building the academic context needed by other pages.
- **`SessionManager.cs`** — Singleton in-memory session store. Keys: `token`, `userData`, `studentInfo`, `academicYears`, `currentAcademicYearId`, `currentAcademicYearName`, `currentTermId`, `currentTermName`, `availableTerms`.
- **`CryptoHelper.cs`** — AES-CBC encrypt/decrypt (key from `AppConfig.ENCRYPTION_KEY`) and HMAC-SHA256 signature generation.
- **`ChangePasswordService.cs`** — Two-step password change: request OTP then submit with OTP.
- **`DashboardService.cs`**, **`GradesService.cs`**, **`ScheduleService.cs`**, **`CourseOfferingsService.cs`** — Page-specific API wrappers.

### Supporting Files
- **`Config/AppConfig.cs`** — Constants: `BASE_URL`, `LOGIN_URL`, `ENCRYPTION_KEY`, `HMAC_SECRET`, `CLIENT_SECRET`, `CLIENT_ID`, `API_TIMEOUT` (30 000 ms).
- **`Exceptions/`** — `ApiException` (with `StatusCode`), `AuthenticationException`, `ValidationException` (with `Field`).
- **`Utils/Validator.cs`** — Input validation; `ValidateStudentId()` enforces `XX-XXXX-XXX` format via source-generated regex.
- **`Assets/`** — `cit-logo.png`, `cit-official-seal.png`. Loaded at runtime from `AppContext.BaseDirectory/Assets/`.
- **`project-requirements/`** — Reference mockup images (not shipped with the app).

## Conventions

- Namespace: `wildcat_one_windows`
- Nullable reference types enabled; implicit usings enabled.
- Two separate base URLs: `AppConfig.LOGIN_URL` (auth/user-info endpoints) and `AppConfig.BASE_URL` (all other student data endpoints). Pass the correct one as `baseUrl` to `ApiService`.
- UI logic lives in `Form1.cs`; avoid hand-editing `Form1.Designer.cs` unless carefully maintaining `InitializeComponent()`.
- Pages track a `_<page>PageLoaded` bool — set it to `false` to force a reload on next visit.
- Brand colors: maroon `Color.FromArgb(122, 26, 61)`, maroon-light `Color.FromArgb(142, 46, 81)`.
