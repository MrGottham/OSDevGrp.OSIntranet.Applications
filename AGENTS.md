# AGENTS.md - Developer Guide for Agentic Coding

## Build & Test Commands

### C# / .NET

```bash
dotnet build OSDevGrp.OSIntranet.Applications.sln
dotnet test OSDevGrp.OSIntranet.Applications.sln

# Single test project
dotnet test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj

# Single test by name
dotnet test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj --filter "FullyQualifiedName~TestMethodName"

# Unit tests only (safe without external services)
dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"

# Watch mode for a test project
dotnet watch test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj
```

### EF Core Migrations (MySQL)

The startup project for migrations is `OSDevGrp.OSIntranet.Repositories.Migration` (standalone executable host):

```bash
dotnet ef migrations add <MigrationName> \
  --project OSDevGrp.OSIntranet.Repositories \
  --startup-project OSDevGrp.OSIntranet.Repositories.Migration

dotnet ef database update \
  --project OSDevGrp.OSIntranet.Repositories \
  --startup-project OSDevGrp.OSIntranet.Repositories.Migration
```

### React / JavaScript

All npm commands run from `osdevgrp.osintranet.react/`:

```bash
npm install
npm run dev        # Vite HMR dev server
npm run build      # Production build
npm run preview    # Preview production build locally
npm run lint       # ESLint only — no test runner configured
```

## Project Structure

Every layer has three companion projects: `*.Interfaces` (contracts), the implementation, and `*.TestHelpers` (shared mock builders for tests). Never put interfaces in the implementation project or test helpers in the `*.Tests` project.

| Directory pattern | Role |
|---|---|
| `OSDevGrp.OSIntranet.Core*` | Shared utilities, `NullGuard`, `IntranetExceptionBuilder` |
| `OSDevGrp.OSIntranet.Domain*` | Domain entities and logic |
| `OSDevGrp.OSIntranet.Repositories*` | EF Core / MySQL data access + migrations |
| `OSDevGrp.OSIntranet.BusinessLogic*` | Application service layer (AutoMapper) |
| `OSDevGrp.OSIntranet.WebApi*` | Main REST API + NSwag-generated client package |
| `OSDevGrp.OSIntranet.Mvc*` | MVC web application |
| `OSDevGrp.OSIntranet.Bff*` | BFF services (DomainServices, ServiceGateways, WebApi — each with own Interfaces/TestHelpers) |
| `osdevgrp.osintranet.react/` | React 19 + Vite 8 frontend |

## Testing Conventions

- Frameworks: **NUnit 4.6.1**, NUnit3TestAdapter 6.2.0, Moq 4.20.72, AutoFixture 4.18.1
- Every test class: `[TestFixture]`, `[SetUp]` creates a fresh `Fixture` and `Random`
- Tests are tagged `[Category("UnitTest")]` or `[Category("IntegrationTest")]`
- Integration tests require live MySQL and external OAuth/Graph services — always filter to `Category=UnitTest` in isolated environments
- Shared mock builders live in `*.TestHelpers` projects (e.g., `_fixture.BuildAuthorizationCodeMock(...)`). Use these instead of building mocks inline
- Abstract `*TestBase` classes group related test fixtures — follow the pattern when adding tests

## Code Style

**C#:**
- `OSDevGrp.OSIntranet.[Layer].[Feature]` namespace must match directory structure
- Interfaces prefix `I`, base classes/interfaces suffix `Base`
- Methods: PascalCase; private fields: `_underscore`; async methods: `MethodAsync()`
- `#region` blocks are **mandatory**: `Constructor`, `Properties`, `Methods`, `Nested classes`
- Guard: `NullGuard.NotNull(param, nameof(param))` / `NullGuard.NotNullOrWhiteSpace(...)`
- Exceptions: `throw new IntranetExceptionBuilder(ErrorCode.Code, context).Build();`
- Catch order: `IntranetExceptionBase` → `AggregateException` → `Exception`
- System imports first, then third-party alphabetically, then local
- Check per-project `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>` in the target `.csproj` — not all projects have both

**React:**
- Components: PascalCase `.jsx` (one per file); utils: camelCase `.js`
- Functional components with hooks only
- ESLint: no unused variables except `^[A-Z_]` pattern
- Use `react-error-boundary` for error boundaries; `yup` for validation; optional chaining throughout

## Toolchain Quirks

**AutoMapper requires a commercial license key.** Version 16.x throws at runtime without `licensesAutoMapperLicenseKey` in environment config. Unit tests that mock AutoMapper are fine; tests that exercise real mapping need the key from `.env`.

**MySQL only — not SQL Server.** ORM is `MySql.EntityFrameworkCore` (Oracle provider). Connection strings and migration commands are MySQL-specific.

**NSwag post-build codegen:** `OSDevGrp.OSIntranet.WebApi.PostBuild` generates `WebApiClient.generated.cs` in `OSDevGrp.OSIntranet.WebApi.ClientApi/` after the WebApi builds. Do not manually edit the generated file. Rebuild order when changing controller signatures:
1. Build `WebApi`
2. Build `WebApi.PostBuild` (regenerates client)
3. Build `WebApi.ClientApi`

**`WebApi.ClientApi` is also a NuGet package** (`GeneratePackageOnBuild=true`). It is referenced as a project reference internally but published as a NuGet package externally.

**No CI automation.** `.github/workflows/` is empty. No pre-commit hooks are active (only `.sample` files).

**Full stack requires Docker Compose.** The `.env` file at the root contains credentials for MySQL, OIDC, Microsoft Graph, Google OAuth, JWT keys, and AutoMapper license. Running individual services with `dotnet run` works; the integrated stack needs `docker-compose up`.
