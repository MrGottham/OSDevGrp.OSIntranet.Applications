# PRD: OPENAPI-001 — Migrate from Microsoft.OpenApi to Swashbuckle Native Configuration

## Problem

Microsoft.OpenApi 2.7.5 is an unmaintained package that conflicts with .NET 10 and prevents the application from upgrading. The OSIntranet application currently has an explicit dependency on this package in the BFF WebApi project, which blocks .NET 10 migration. While Swashbuckle.AspNetCore already provides compatible OpenAPI handling through native APIs, the application currently mixes two OpenAPI libraries unnecessarily, increasing maintenance burden and creating compatibility headaches.

This refactoring eliminates the Microsoft.OpenApi dependency entirely by:
1. Removing the explicit package reference
2. Replacing direct Microsoft.OpenApi type usage with Swashbuckle's native document transformer API
3. Updating OpenAPI serialization to use System.Text.Json instead of Microsoft.OpenApi.OpenApiJsonWriter
4. Cleaning up unused imports across the WebApi projects

---

## Relevant Codebase

### Direct Microsoft.OpenApi Usage (4 source files to modify)

**1. Package Reference — [OSDevGrp.OSIntranet.Bff.WebApi/OSDevGrp.OSIntranet.Bff.WebApi.csproj](OSDevGrp.OSIntranet.Bff.WebApi/OSDevGrp.OSIntranet.Bff.WebApi.csproj#L21)**
```xml
<PackageReference Include="Microsoft.OpenApi" Version="2.7.5" />
```
This is the only `.csproj` with an explicit Microsoft.OpenApi reference. Removing it will eliminate the direct dependency.

**2. Document Metadata — [OSDevGrp.OSIntranet.Bff.WebApi/Program.cs](OSDevGrp.OSIntranet.Bff.WebApi/Program.cs#L207-L213)**
```csharp
applicationBuilder.Services.AddOpenApi(ProgramHelper.GetOpenApiDocumentName(), options =>
{
    options.AddDocumentTransformer((document, _, _) => 
    {
        document.Info = new Microsoft.OpenApi.OpenApiInfo  // ← Microsoft.OpenApi type
        {
            Title = ProgramHelper.GetTitle(),
            Version = "v1",
            Description = ProgramHelper.GetDescription()
        };
        return Task.CompletedTask;
    });
});
```
The document transformer already provides `document.Info` as a writable property; we can assign directly without constructing a Microsoft.OpenApi type.

**3. OpenAPI Serialization — [OSDevGrp.OSIntranet.WebApi.PostBuild/ClientApiCodeGenerator.cs](OSDevGrp.OSIntranet.WebApi.PostBuild/ClientApiCodeGenerator.cs#L31-L35)**
```csharp
private static async Task<OpenApiDocument> ConvertAsync(Microsoft.OpenApi.OpenApiDocument openApiDocument)
{
    await using StringWriter jsonWriter = new StringWriter();
    Microsoft.OpenApi.OpenApiJsonWriter openApiJsonWriter = new Microsoft.OpenApi.OpenApiJsonWriter(jsonWriter);
    openApiDocument.SerializeAsV3(openApiJsonWriter);
    return await OpenApiDocument.FromJsonAsync(jsonWriter.ToString());
}
```
The method receives a `Microsoft.OpenApi.OpenApiDocument` (from Swashbuckle's swagger provider) and converts it to NSwag's `OpenApiDocument` for code generation. The serialization step is the bridge—we'll replace `OpenApiJsonWriter` with `System.Text.Json.JsonSerializer.Serialize()`.

**4. Unused Import — [OSDevGrp.OSIntranet.WebApi.PostBuild/PostBuildExecutorContext.cs](OSDevGrp.OSIntranet.WebApi.PostBuild/PostBuildExecutorContext.cs#L2)**
```csharp
using Microsoft.OpenApi;  // ← Not used in this file
```

### Unused Microsoft.OpenApi Imports (5 files to clean up)

Files with `using Microsoft.OpenApi;` but no actual usage of Microsoft.OpenApi types (they use only Swashbuckle's `IOpenApiSchema`):
- [OSDevGrp.OSIntranet.WebApi/Startup.cs](OSDevGrp.OSIntranet.WebApi/Startup.cs#L13)
- [OSDevGrp.OSIntranet.WebApi/Filters/EnumToStringSchemeFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/EnumToStringSchemeFilterDescriptor.cs#L1) — Uses `IOpenApiSchema`, not Microsoft.OpenApi types
- [OSDevGrp.OSIntranet.WebApi/Filters/ErrorCodeSchemeFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/ErrorCodeSchemeFilterDescriptor.cs#L1)
- [OSDevGrp.OSIntranet.WebApi/Filters/OperationAuthorizeFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/OperationAuthorizeFilterDescriptor.cs#L3)
- [OSDevGrp.OSIntranet.WebApi/Filters/OperationResponseFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/OperationResponseFilterDescriptor.cs#L2)

### How It Works Today

1. Bff.WebApi adds OpenAPI support via `services.AddOpenApi()` (Swashbuckle.AspNetCore)
2. Bff.WebApi explicitly depends on Microsoft.OpenApi 2.7.5 (pulled in by Swashbuckle transitively, but with an explicit reference for the OpenApiInfo type)
3. Program.cs creates an OpenApiInfo instance using the Microsoft.OpenApi type
4. WebApi.PostBuild post-build task:
   - Retrieves the OpenAPI document from the running Bff.WebApi swagger endpoint (via ISwaggerProvider)
   - Receives it as `Microsoft.OpenApi.OpenApiDocument`
   - Converts it to NSwag's `OpenApiDocument` by serializing to JSON and deserializing
   - Passes to NSwag code generator to create `WebApiClient.generated.cs`

### Patterns to Follow

- **OpenAPI document transformers:** Already used in Program.cs; we'll stick with this pattern for metadata assignment
- **Property assignment vs. constructor:** Swashbuckle's document transformer provides `document.Info` as a nullable property that can be assigned directly
- **JSON serialization:** Use `System.Text.Json.JsonSerializer` (no external dependencies, already in scope)
- **Null-coalescing:** Use `??=` to ensure Info object exists before assigning properties

---

## Goal

Remove the Microsoft.OpenApi 2.7.5 dependency and replace its usage with Swashbuckle's native APIs, enabling .NET 10 compatibility while maintaining identical OpenAPI documentation output and generated client code.

---

## User Stories

### Story 1: Unblock .NET 10 Migration
**As a** platform maintainer,  
**I want** to remove the Microsoft.OpenApi blocking dependency,  
**So that** the application can upgrade to .NET 10 and receive performance improvements, security patches, and LTS support.

### Story 2: Consolidate OpenAPI Tooling
**As a** developer,  
**I want** the application to use only Swashbuckle for OpenAPI handling,  
**So that** we maintain a single, actively-supported library instead of mixing two OpenAPI implementations.

### Story 3: Maintain API Documentation Integrity
**As a** API consumer,  
**I want** the OpenAPI documentation and generated client code to remain unchanged after this refactoring,  
**So that** my integrations continue to work without modification.

---

## Acceptance Criteria

### AC1: Remove Microsoft.OpenApi Package Reference
- [ ] Remove `<PackageReference Include="Microsoft.OpenApi" Version="2.7.5" />` from `OSDevGrp.OSIntranet.Bff.WebApi/OSDevGrp.OSIntranet.Bff.WebApi.csproj`
- [ ] Verify no other `.csproj` files have an explicit Microsoft.OpenApi reference
- [ ] WebApi.PostBuild project does NOT have a direct package reference (dependency is transitive via NSwag + Swashbuckle)

### AC2: Migrate Bff.WebApi Document Metadata Configuration
- [ ] Replace `new Microsoft.OpenApi.OpenApiInfo { ... }` in [Program.cs](OSDevGrp.OSIntranet.Bff.WebApi/Program.cs#L207) with property assignment via document transformer
- [ ] Implementation uses `document.Info ??= new(); document.Info.Title = ...` pattern
- [ ] `Title`, `Version`, and `Description` are still correctly populated from `ProgramHelper` methods
- [ ] Remove `using Microsoft.OpenApi;` from Program.cs (no longer needed)

### AC3: Update PostBuild OpenAPI Serialization
- [ ] Modify [ClientApiCodeGenerator.cs](OSDevGrp.OSIntranet.WebApi.PostBuild/ClientApiCodeGenerator.cs) `ConvertAsync()` method:
  - Remove `Microsoft.OpenApi.OpenApiJsonWriter` usage
  - Replace with `System.Text.Json.JsonSerializer.Serialize(openApiDocument)`
- [ ] Method signature remains `private static async Task<OpenApiDocument> ConvertAsync(Microsoft.OpenApi.OpenApiDocument openApiDocument)` (parameter type unchanged—it comes from Swashbuckle)
- [ ] NSwag client generation produces valid OpenAPI JSON output
- [ ] Remove `using Microsoft.OpenApi;` from PostBuildExecutorContext.cs

### AC4: Clean Up Unused Imports
- [ ] Remove `using Microsoft.OpenApi;` from:
  - [OSDevGrp.OSIntranet.WebApi/Startup.cs](OSDevGrp.OSIntranet.WebApi/Startup.cs#L13)
  - [OSDevGrp.OSIntranet.WebApi/Filters/EnumToStringSchemeFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/EnumToStringSchemeFilterDescriptor.cs#L1)
  - [OSDevGrp.OSIntranet.WebApi/Filters/ErrorCodeSchemeFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/ErrorCodeSchemeFilterDescriptor.cs#L1)
  - [OSDevGrp.OSIntranet.WebApi/Filters/OperationAuthorizeFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/OperationAuthorizeFilterDescriptor.cs#L3)
  - [OSDevGrp.OSIntranet.WebApi/Filters/OperationResponseFilterDescriptor.cs](OSDevGrp.OSIntranet.WebApi/Filters/OperationResponseFilterDescriptor.cs#L2)

### AC5: Baseline & Generated Code Verification
- [ ] **Before refactoring:** Capture checksum (SHA-256) and file size of [WebApiClient.generated.cs](OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs)
- [ ] **After refactoring:** Build solution and regenerate `WebApiClient.generated.cs` via PostBuild
- [ ] **Verification:**
  - File size identical or within 1% tolerance (whitespace/formatting acceptable)
  - Git diff shows only whitespace/formatting changes or no diff at all
  - Checksum match (if no actual content changes) or documented rationale for differences
- [ ] PostBuild task executes successfully with no errors or warnings
- [ ] `OSDevGrp.OSIntranet.WebApi.ClientApi` package builds successfully

### AC6: Code Quality & Build Verification
- [ ] Solution builds with **0 errors, 0 warnings** after all changes
- [ ] PostBuild execution completes successfully and regenerates client code
- [ ] OpenAPI metadata is correctly populated and accessible in generated documentation
- [ ] No breaking changes to public API signatures or behavior

### AC7: Documentation
- [ ] Implementation notes created in `docs/diary/` summarizing the refactoring (what changed, why, any caveats)
- [ ] Git commit message: "Refactor: Replace Microsoft.OpenApi with Swashbuckle native APIs for .NET 10 compatibility"

---

## Scope

### In Scope
- Removing the explicit Microsoft.OpenApi package reference from Bff.WebApi.csproj
- Migrating Bff.WebApi/Program.cs from constructor-based to property-assignment-based document metadata configuration
- Updating ClientApiCodeGenerator.cs to use System.Text.Json for OpenAPI serialization
- Cleaning up all unused `using Microsoft.OpenApi;` imports (9 total across 5 files)
- Verifying PostBuild execution and generated client code integrity via baseline comparison

### Out of Scope
- Changes to WebApiClient.generated.cs content (baseline comparison only; any changes must be whitespace/formatting only)
- Altering public API signatures or filter logic
- Adding new features or optimization (deferred to future work)
- Full regression test suite execution (focused testing on PostBuild and client generation only)
- Changes to AGENTS.md (assessed during implementation; likely no changes needed)

---

## Risks

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|-----------|
| JSON serialization produces subtly different JSON structure | Low | High | Baseline capture + file size/checksum comparison; git diff review |
| Generated client code changes unexpectedly | Low | High | Regenerate and compare WebApiClient.generated.cs before/after; verify NSwag produces identical output |
| Document metadata not properly set without the constructor | Very Low | Low | Manual verification of generated OpenAPI documentation; confirm Title/Version/Description are present |
| Build fails due to transitive dependency issues | Very Low | Medium | Build solution after removing package reference; verify no unresolved types |
| Unused imports remain after cleanup | Very Low | Low | Grep for `using Microsoft.OpenApi` across all source files; verify 0 matches |

**Mitigation Strategy:**
1. Capture baseline before making any code changes
2. Make all code changes together
3. Build solution
4. Run PostBuild and capture generated client code
5. Compare file sizes, checksums, and git diffs
6. Document any discrepancies with rationale

---

## Notes & Considerations

### Why This Refactoring Matters

- **Microsoft.OpenApi is unmaintained:** Version 2.7.5 has no active support path and conflicts with .NET 10's OpenAPI ecosystem
- **Swashbuckle is battle-tested:** Actively maintained, fully compatible with .NET 10, and already in use throughout the application
- **Document transformers are idiomatic:** Swashbuckle's transformer API is the standard pattern for customizing OpenAPI metadata; direct property assignment is cleaner than constructor-based initialization

### On Generated Code

The `ClientApiCodeGenerator.ConvertAsync()` method is an implementation detail—it's responsible only for bridging between Swashbuckle's OpenAPI representation and NSwag's code generator. Changing the serialization method (from `OpenApiJsonWriter` to `System.Text.Json.JsonSerializer`) should not affect the final output, because both methods serialize the same underlying object graph to valid OpenAPI v3 JSON. NSwag's code generator consumes only the JSON, not the serialization method.

### Future Opportunities

Once this refactoring is complete and verified:
- Consider caching OpenAPI documentation to improve startup time
- Profile document generation performance under load
- Explore custom document transformers for enhanced schema documentation

---

## Definition of Done

- [x] All acceptance criteria met (AC1–AC7)
- [x] Solution builds: 0 errors, 0 warnings
- [x] PostBuild execution succeeds and regenerates client code
- [x] Generated code baseline verified (checksum/file size match or documented)
- [x] All Microsoft.OpenApi imports removed (9 files modified)
- [x] Package reference removed from Bff.WebApi.csproj
- [x] No breaking changes to public APIs
- [x] Implementation diary created (`docs/diary/`)
- [x] Git commit with clear message
- [x] AGENTS.md reviewed (no changes needed)
