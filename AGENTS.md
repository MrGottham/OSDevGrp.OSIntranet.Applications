# AGENTS.md - Developer Guide for Agentic Coding

## Build & Test Commands

### C# / .NET

**Build entire solution:**
```bash
dotnet build OSDevGrp.OSIntranet.Applications.sln
```

**Run all tests:**
```bash
dotnet test OSDevGrp.OSIntranet.Applications.sln
```

**Run specific test project:**
```bash
dotnet test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj
```

**Run single test by name:**
```bash
dotnet test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj --filter "FullyQualifiedName~TestMethodName"
```

**Watch mode (auto-rebuild on file changes):**
```bash
dotnet watch build
```
or run a specific test project in watch mode:
```bash
dotnet watch test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj
```

### React / JavaScript

Navigate to `osdevgrp.osintranet.react/` directory for all npm commands.

**Install dependencies:**
```bash
npm install
```

**Development server (with HMR):**
```bash
npm run dev
```

**Build for production:**
```bash
npm run build
```

**Lint code:**
```bash
npm run lint
```

**Note:** React app has no test runner configured (only ESLint linting).

## Code Style

### C# Specific

**Namespace & structure:**
- `OSDevGrp.OSIntranet.[Layer].[Feature]` must match directory structure exactly
- Interfaces prefix with `I`, base classes/interfaces suffix with `Base`
- Methods: PascalCase, private fields: `_underscore`, async methods: `MethodAsync()`

**Validation & exceptions:**
- Use `NullGuard.NotNull(param, nameof(param))` from Core module
- Use `NullGuard.NotNullOrWhiteSpace(param, nameof(param))` for strings
- Use `IntranetExceptionBuilder` for exceptions: `throw new IntranetExceptionBuilder(ErrorCode.Code, context).Build();`
- Catch order: `IntranetExceptionBase` → `AggregateException` → `Exception`

**Organization:**
- Use `#region` markers: Constructor, Properties, Methods, Nested classes
- System imports first, then third-party alphabetically, then local imports

### React Specific

**File & naming:**
- Components: PascalCase `.jsx` (one component per file)
- Utils: camelCase `.js`
- Functional components with hooks only
- ESLint rules: no unused variables except `^[A-Z_]` pattern

**Error handling:**
- Use `react-error-boundary` for component error boundaries
- Validate with `yup` schemas
- Use optional chaining: `obj?.property?.nested`

## Project Structure

- `OSDevGrp.OSIntranet.Domain*` - Business domain entities and logic
- `OSDevGrp.OSIntranet.Repositories*` - Data access layer
- `OSDevGrp.OSIntranet.Core*` - Shared utilities and helpers
- `OSDevGrp.OSIntranet.BusinessLogic*` - Application service layer
- `OSDevGrp.OSIntranet.WebApi*` - REST API controllers
- `OSDevGrp.OSIntranet.Mvc*` - MVC application
- `OSDevGrp.OSIntranet.Bff*` - Backend-for-Frontend services
- `osdevgrp.osintranet.react/` - React frontend application

## Testing Frameworks

- **C#**: NUnit 4.5.1, Moq 4.20.72, AutoFixture 4.18.1
- **JavaScript**: ESLint with React plugin (no test runner)

## Key Dependencies

- **Backend**: ASP.NET Core, Entity Framework, AutoMapper
- **Frontend**: React 19, React Router 7, Formik/Yup for forms, React Bootstrap for UI

## Important Notes

- All C# projects target .NET 10.0
- Solution uses directory-based namespace structure
- Tests are organized in parallel project structure (e.g., `*.Tests` suffix)
- React frontend is a separate npm project with independent build
- Use VSCode tasks for build/publish/watch operations
