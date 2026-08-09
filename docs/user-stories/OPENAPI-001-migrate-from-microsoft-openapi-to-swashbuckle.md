# User Story: Migrate from Microsoft.OpenApi to Swashbuckle Native Configuration for .NET 10 Compatibility

## Story ID
OPENAPI-001

## Title
Refactor OpenAPI documentation configuration to use Swashbuckle native APIs and remove Microsoft.OpenApi 2.7.5 dependency for .NET 10 compatibility

## Epic
.NET 10 Migration

## Type
Technical Debt / Refactoring

---

## Business Value

**Why This Matters:**
- **Unblocks .NET 10 Migration**: Resolves compatibility issue preventing upgrade to .NET 10, which includes performance improvements, security patches, and LTS support
- **Reduces Maintenance Burden**: Eliminates dependency on unmaintained Microsoft.OpenApi package that conflicts with .NET 10
- **Improves Maintainability**: Standardizes on Swashbuckle (already in use) rather than mixing OpenAPI libraries
- **Future-Proofs**: Aligns with ecosystem best practices; Swashbuckle is actively maintained and fully compatible with latest .NET versions
- **Zero User Impact**: Refactoring is internal only; API documentation output remains unchanged

**Impact Scope:** 
- **1 project with direct package reference**: OSDevGrp.OSIntranet.Bff.WebApi (contains explicit `<PackageReference>`)
- **1 project with code dependency**: OSDevGrp.OSIntranet.WebApi.PostBuild (uses Microsoft.OpenApi types via transitive dependency; code must be refactored)
- 5 files modified
- No breaking changes to API consumers

---

## User Story Description

As a **developer maintaining the OSIntranet application**,  
I want to **replace Microsoft.OpenApi explicit usage with Swashbuckle's native configuration**,  
So that **the application can successfully target .NET 10 and maintain API documentation generation without external dependency conflicts**.

---

## Acceptance Criteria

### AC1: Remove Microsoft.OpenApi Package Reference from Bff.WebApi
- [ ] Remove `<PackageReference Include="Microsoft.OpenApi" Version="2.7.5" />` from `OSDevGrp.OSIntranet.Bff.WebApi.csproj`
- [ ] Verify Bff.WebApi is the only `.csproj` with an explicit Microsoft.OpenApi reference
- [ ] Confirm PostBuild project does not have a direct package reference (dependency is transitive via Swashbuckle)
- [ ] Solution builds successfully without warnings after removal

### AC2: Migrate Bff.WebApi Document Metadata Configuration
- [ ] Replace `new Microsoft.OpenApi.OpenApiInfo { ... }` in `OSDevGrp.OSIntranet.Bff.WebApi/Program.cs` (line 207) with Swashbuckle document transformer
- [ ] Verify that `Title`, `Version`, and `Description` are still populated correctly via `ProgramHelper` methods
- [ ] Confirm the generated OpenAPI documentation contains the same metadata

### AC3: Update PostBuild OpenAPI Serialization
- [ ] Modify `ClientApiCodeGenerator.cs` `ConvertAsync()` method to serialize OpenAPI document without `Microsoft.OpenApi.OpenApiJsonWriter`
- [ ] Replace `Microsoft.OpenApi.OpenApiJsonWriter` usage with `System.Text.Json.JsonSerializer.Serialize()`
- [ ] Verify NSwag client generation produces valid OpenAPI JSON output

### AC3a: Verify Generated Client Code Integrity
- [ ] Before refactoring: Capture baseline of `WebApiClient.generated.cs` (file size, checksum, structure)
- [ ] After refactoring: Regenerate `WebApiClient.generated.cs` during build
- [ ] Compare generated files:
  - [ ] File size is identical or within 1% tolerance (whitespace/formatting variations acceptable)
  - [ ] All public classes, methods, and properties match exactly
  - [ ] No changes to method signatures or parameter types
  - [ ] No changes to property types or validation attributes
  - [ ] Git diff shows only whitespace or trivial formatting differences (if any)
- [ ] Verify NSwag code generation produces no errors or warnings
- [ ] Confirm `OSDevGrp.OSIntranet.WebApi.ClientApi` package can be built and published without issues

### AC4: Remove Unused Namespace Imports
- [ ] Remove `using Microsoft.OpenApi;` from:
  - `OSDevGrp.OSIntranet.WebApi.PostBuild/PostBuildExecutorContext.cs`
  - `OSDevGrp.OSIntranet.Bff.WebApi/Program.cs` (after migration)
- [ ] Confirm no other WebApi filter classes (EnumToStringSchemeFilterDescriptor, ErrorCodeSchemeFilterDescriptor, etc.) are affected — they should continue using Swashbuckle's IOpenApiSchema (which is compatible)

### AC5: Code Quality & Testing
- [ ] Solution builds with 0 errors, 0 warnings
- [ ] All unit tests pass (verify no regression in test count or results)
- [ ] Run full test suite: `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"`
- [ ] Manually verify OpenAPI documentation is generated and accessible at `/swagger/v1/swagger.json` (or equivalent)
- [ ] No breaking changes to public APIs

### AC6: Documentation
- [ ] Update AGENTS.md if Microsoft.OpenApi toolchain notes are present
- [ ] Create implementation notes in docs/diary/ summarizing the refactoring
- [ ] Document any breaking changes (none expected) or migration notes for other developers

---

## Technical Details

### Implementation Approach: Option 1 (Recommended)

**Step 1: Remove Package Reference**
```xml
<!-- Remove from OSDevGrp.OSIntranet.Bff.WebApi.csproj -->
<PackageReference Include="Microsoft.OpenApi" Version="2.7.5" />
```

**Step 2: Migrate Document Info Configuration (Program.cs)**
```csharp
// Before:
applicationBuilder.Services.AddOpenApi(ProgramHelper.GetOpenApiDocumentName(), options =>
{
    options.AddDocumentTransformer((document, _, _) => 
    {
        document.Info = new Microsoft.OpenApi.OpenApiInfo
        {
            Title = ProgramHelper.GetTitle(),
            Version = "v1",
            Description = ProgramHelper.GetDescription()
        };
        return Task.CompletedTask;
    });
});

// After:
applicationBuilder.Services.AddOpenApi(ProgramHelper.GetOpenApiDocumentName(), options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info ??= new();
        document.Info.Title = ProgramHelper.GetTitle();
        document.Info.Version = "v1";
        document.Info.Description = ProgramHelper.GetDescription();
        return Task.CompletedTask;
    });
});
```

**Step 3: Update PostBuild Serialization (ClientApiCodeGenerator.cs)**
```csharp
// Before:
private static async Task<OpenApiDocument> ConvertAsync(Microsoft.OpenApi.OpenApiDocument openApiDocument)
{
    await using StringWriter jsonWriter = new StringWriter();
    Microsoft.OpenApi.OpenApiJsonWriter openApiJsonWriter = new Microsoft.OpenApi.OpenApiJsonWriter(jsonWriter);
    openApiDocument.SerializeAsV3(openApiJsonWriter);
    return await OpenApiDocument.FromJsonAsync(jsonWriter.ToString());
}

// After:
private static async Task<OpenApiDocument> ConvertAsync(Microsoft.OpenApi.OpenApiDocument openApiDocument)
{
    string jsonContent = System.Text.Json.JsonSerializer.Serialize(openApiDocument);
    return await OpenApiDocument.FromJsonAsync(jsonContent);
}
```

**Step 4: Verify WebApi Filter Compatibility**
- Filter classes (`EnumToStringSchemeFilterDescriptor`, `ErrorCodeSchemeFilterDescriptor`, `OperationAuthorizeFilterDescriptor`, `OperationResponseFilterDescriptor`) use Swashbuckle's `IOpenApiSchema` interface, which is independent of Microsoft.OpenApi package
- These should continue working without modification

---

## Dependencies

### Required Packages
- ✅ `Swashbuckle.AspNetCore` 10.2.3 (already present, already .NET 10 compatible)
- ✅ `Microsoft.AspNetCore.OpenApi` 10.0.10 (already present, .NET 10 compatible)
- ✅ `NSwag.CodeGeneration.CSharp` 14.7.1 (already present, compatible)

### No Additional Dependencies
No new NuGet packages required. All infrastructure already exists.

---

## Risks & Mitigation

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|-----------|
| JSON serialization method change produces different JSON output | Low | High | AC3a: Capture baseline and verify generated `WebApiClient.generated.cs` is byte-for-byte identical (whitespace tolerant) |
| Generated client code differs from baseline | Low | High | AC3a: Git diff comparison + manual code review of any generated changes |
| Document metadata not properly applied | Low | Medium | Manual verification of `/swagger/v1/swagger.json` endpoint; compare before/after |
| Swashbuckle document transformer API change in future versions | Very Low | Medium | Pin Swashbuckle version to 10.2.3; monitor release notes |
| Regression in existing WebApi filter logic | Very Low | Medium | Full unit test suite pass; focus on schema filter tests |

**Mitigation Strategy:** 
- Generate OpenAPI docs in dev environment before/after refactoring and compare JSON output
- Run full unit test suite including WebApi tests including PostBuild tests
- Baseline and regenerate `WebApiClient.generated.cs` to verify no functional changes
- Perform smoke test of client code generation post-build
- Verify `OSDevGrp.OSIntranet.WebApi.ClientApi` NuGet package generation succeeds

---

## Effort Estimate

| Task | Estimated Time |
|------|-----------------|
| Remove package reference and update csproj | 5 min |
| Migrate Program.cs document metadata configuration | 10 min |
| Update ClientApiCodeGenerator serialization | 15 min |
| Remove unused imports and clean up | 5 min |
| Capture baseline of WebApiClient.generated.cs | 5 min |
| Full solution build and test verification | 20 min |
| Regenerate and verify generated client code (AC3a) | 10 min |
| Documentation and commit | 10 min |
| **Total** | **~80 minutes (1 hour 20 minutes)** |

**Complexity:** Low  
**Risk Level:** Low  
**Testing Effort:** Medium (requires full test suite run + baseline comparison of generated code)

---

## Definition of Done

- [ ] All acceptance criteria met (including AC3a for generated code verification)
- [ ] Solution builds: 0 errors, 0 warnings
- [ ] Full unit test suite passes: `dotnet test ... --filter "Category=UnitTest"` → 17,559+ tests pass
- [ ] No regressions in existing test results
- [ ] OpenAPI documentation generated correctly and accessible at `/swagger/v1/swagger.json`
- [ ] **Generated code verified**: `WebApiClient.generated.cs` is identical (or functionally equivalent with only whitespace/formatting differences)
- [ ] `OSDevGrp.OSIntranet.WebApi.ClientApi` NuGet package builds and publishes successfully
- [ ] Code reviewed and approved
- [ ] Implementation diary created (docs/diary/)
- [ ] Git commit with clear message: "Refactor: Replace Microsoft.OpenApi with Swashbuckle native APIs for .NET 10 compatibility"
- [ ] AGENTS.md updated if necessary

---

## Notes & Considerations

### Why Swashbuckle's Approach is Superior
1. **Single Source of Truth**: One actively-maintained library (Swashbuckle) instead of two (Swashbuckle + Microsoft.OpenApi)
2. **Built-In Ecosystem**: Document transformers, operation filters, and schema filters are designed to work together seamlessly
3. **Future Compatibility**: Swashbuckle team ensures compatibility with new .NET releases; Microsoft.OpenApi has a maintenance backlog
4. **Performance**: Eliminates unnecessary dependency graph complexity

### Impact on External Consumers
- **None**: API documentation endpoint output should remain identical
- **None**: WebApi client code generation remains unchanged
- **None**: Swagger UI experience unchanged

### Future Enhancements
Once this refactoring is complete, consider:
- Caching OpenAPI documentation to improve startup time
- Adding custom document transformers for enhanced schema documentation
- Performance profiling of document generation during startup

---

## Related Issues/PRs
- .NET 10 Upgrade Tracking Issue
- Microsoft.OpenApi compatibility blocker

## Assignee
[To be assigned]

## Sprint
[To be assigned]

## Labels
`technical-debt`, `.net-10-migration`, `refactoring`, `low-priority`, `internal-only`
