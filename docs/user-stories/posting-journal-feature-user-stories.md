# User Stories: Posting Journal Feature

**Epic:** Expose Posting Journal for Accounting via BFF WebApi

**Date:** 2026-08-14

**Status:** Ready for Implementation

---

## Epic: Expose Posting Journal for Accounting via BFF WebApi

**Business Value:** Enable the React application to retrieve and display the posting journal for a given accounting, supporting the ability to reload the posting journal when users add, update, or delete posting lines within the React UI.

**Scope:**
- 12 user stories spanning 3 layers: DomainServices, DomainServices.Tests, and WebApi
- Estimated effort: 3-5 developer days
- Dependencies: All required components (interfaces, builders, gateway methods) already exist in the codebase

**Key Technical Decision:** Uses `IsAccountingModifier` permission check instead of `IsAccountingViewer` because the posting journal is accessed for write operations (adding/updating/deleting posting lines), not just reading.

---

## User Story 1: Create PostingJournalRequest Query Request Class

**As a** Backend Developer
**I want to** Create a `PostingJournalRequest` class for the posting journal query feature
**So that** The query feature layer has a properly typed request object that extends the established accounting request pattern

### Acceptance Criteria:
- [ ] Class inherits from `AccountingIdentificationRequestBase`
- [ ] Created in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/` 
- [ ] Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal`
- [ ] Includes proper constructor chaining to base class
- [ ] Inherits `AccountingNumber`, `StatusDate`, `SecurityContext` properties from base
- [ ] Follows project naming conventions and #region blocks

### Technical Details:
- Inherits from `AccountingIdentificationRequestBase`
- Constructor: `public PostingJournalRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext) : base(...)`
- Approximately 10 lines of code

---

## User Story 2: Create PostingJournalResponse Query Response Class

**As a** Backend Developer
**I want to** Create a `PostingJournalResponse` class to return posting journal data
**So that** The query feature layer has a properly typed response object with convenience properties

### Acceptance Criteria:
- [ ] Class inherits from `AccountingIdentificationResponseBase<ApplyPostingJournalModel, IPostingJournalTexts>`
- [ ] Created in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/`
- [ ] Has convenience property: `public ApplyPostingJournalModel PostingJournal => Model;`
- [ ] Constructor accepts: model, posting journal texts, static texts, validation rule set
- [ ] All properties properly initialized via base class

### Technical Details:
- Inherits from `AccountingIdentificationResponseBase<ApplyPostingJournalModel, IPostingJournalTexts>`
- Reuses `ApplyPostingJournalModel` from WebApi.ClientApi (already generated)
- Reuses `IPostingJournalTexts` interface (already exists)
- Approximately 18 lines of code

---

## User Story 3: Implement PostingJournalFeature Query Handler

**As a** Backend Developer
**I want to** Implement the `PostingJournalFeature` query handler with proper permission override
**So that** The posting journal query is executed with correct business logic, data retrieval, and write-access permission validation

### Acceptance Criteria:
- [ ] Class inherits from `AccountingIdentificationFeatureBase<PostingJournalRequest, PostingJournalResponse, ApplyPostingJournalModel, IPostingJournalTexts, IPostingJournalTextsBuilder, IPostingJournalRuleSetBuilder>`
- [ ] Created in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/`
- [ ] Marked as `internal` class
- [ ] `GetModelAsync()` calls `IAccountingGateway.GetPostingJournalAsync()` with correct parameters
- [ ] `BuildResponseAsync()` returns new `PostingJournalResponse` with all components
- [ ] `GetStaticTextSpecifications()` includes all 25 static text keys
- [ ] `VerifyPermissionAsync()` override checks: `IsAuthenticated` AND `HasAccountingAccess` AND `IsAccountingModifier` (NOT viewer)
- [ ] Auto-discovered and registered via `AddFeatures()` mechanism
- [ ] All constructor dependencies properly injected: `IPermissionChecker`, `IAccountingGateway`, `IStaticTextProvider`, `IPostingJournalTextsBuilder`, `IPostingJournalRuleSetBuilder`

### Technical Details:
- Uses 6 generic type parameters from base class
- Reuses `PostingJournalTextsBuilder` (already registered in DI)
- Reuses `PostingJournalRuleSetBuilder` (already registered in DI)
- Static text keys (25 total):
  - **Field headers:** PostingJournal, PostingDate, PostingReference, Account, PostingText, BudgetAccount, Debit, Credit, ContactAccount
  - **Field labels:** AccountName, Posted, Available, Balance, PostingValue
  - **Action texts:** AddPostingJournalLine, UpdatePostingJournalLine, DeletePostingJournalLine, PostingJournalLineDeletionQuestion
  - **Dialog/button texts:** Create, Update, Delete, ConfirmDeletion, DeleteVerificationInfo, Reset, Cancel
- Approximately 52 lines of code
- **KEY DIFFERENCE:** Uses `IsAccountingModifier()` instead of `IsAccountingViewer()` for write-operation authorization

---

## User Story 4: Create ExecuteAsyncTests for PostingJournalFeature

**As a** QA/Test Developer
**I want to** Create comprehensive tests for `PostingJournalFeature.ExecuteAsync()` method
**So that** The query feature correctly retrieves data, builds responses, and handles all static text keys

### Acceptance Criteria:
- [ ] Test class: `ExecuteAsyncTests` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/PostingJournal/PostingJournalFeature/`
- [ ] Test: `GetPostingJournalAsync` called with correct `AccountingNumber`
- [ ] Test: `GetPostingJournalAsync` called with correct `CancellationToken`
- [ ] Test: Response `PostingJournal` equals gateway model
- [ ] Test: Response `DynamicTexts` equals builder output
- [ ] Test: Response `ValidationRuleSet` equals builder output
- [ ] Test: All 25 `StaticTextKey` values requested in `GetStaticTextSpecifications()`
- [ ] All tests marked `[Category("UnitTest")]`
- [ ] All tests use NUnit assertions and Moq mocks
- [ ] Fixture and Random initialized in SetUp

### Technical Details:
- 6 test methods total
- Follow NUnit 4.6.1 patterns
- Use AutoFixture for test data generation
- Mock `IAccountingGateway`, `IPostingJournalTextsBuilder`, `IPostingJournalRuleSetBuilder`, `IStaticTextProvider`, `IPermissionChecker`

---

## User Story 5: Create VerifyPermissionAsyncTests for PostingJournalFeature

**As a** QA/Test Developer
**I want to** Create comprehensive tests for `PostingJournalFeature.VerifyPermissionAsync()` override
**So that** Write-access permissions are properly validated with `IsAccountingModifier` (not viewer)

### Acceptance Criteria:
- [ ] Test class: `VerifyPermissionAsyncTests` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/PostingJournal/PostingJournalFeature/`
- [ ] Test: `User` accessed from `ISecurityContext`
- [ ] Test: `IsAuthenticated` called on `IPermissionChecker`
- [ ] Test: `HasAccountingAccess` called on `IPermissionChecker`
- [ ] Test: `IsAccountingModifier` called (KEY DIFFERENCE: not `IsAccountingViewer`)
- [ ] Test: Returns `false` when not authenticated
- [ ] Test: Returns `false` when no accounting access
- [ ] Test: Returns `false` when not accounting modifier
- [ ] Test: Returns `true` when all permissions granted
- [ ] All tests marked `[Category("UnitTest")]`
- [ ] All tests use NUnit assertions and Moq mocks

### Technical Details:
- 8 test methods total
- Verify permission chain: IsAuthenticated → HasAccountingAccess → IsAccountingModifier
- Mock `IPermissionChecker` and `ISecurityContext`
- Critical test: Ensure `IsAccountingModifier` is called, NOT `IsAccountingViewer`

---

## User Story 6: Add AccountingModifier Authorization Policy to WebApi

**As a** Backend Developer
**I want to** Add the `AccountingModifier` authorization policy to the BFF WebApi
**So that** The posting journal endpoint requires write-level accounting permissions instead of read-level

### Acceptance Criteria:
- [ ] Add `AccountingModifier` constant to `OSDevGrp.OSIntranet.Bff.WebApi/Security/Policies.cs`
  - [ ] Constant value: `"AccountingModifier"`
- [ ] Add `AccountingModifier` policy configuration to `OSDevGrp.OSIntranet.Bff.WebApi/Program.cs`
  - [ ] Positioned after `AccountingViewer` policy
  - [ ] Uses `Schemes.Internal` authentication scheme
  - [ ] Requires authenticated user
  - [ ] Requires claims: NameIdentifier, Name, Email, AccountingClaimType, AccountingModifierClaimType
  - [ ] Properly formatted code block follows existing policy patterns

### Technical Details:
- Policy requires `OSDevGrp.OSIntranet.Bff.DomainServices.Security.ClaimTypes.AccountingModifierClaimType`
- Differs from `AccountingViewer` which requires `AccountingViewerClaimType`
- Follows authorization builder fluent API patterns
- Required claims are cumulative (must have all of them)

---

## User Story 7: Create PostingJournalResponseDto DTO for WebApi Response

**As a** Backend Developer
**I want to** Create `PostingJournalResponseDto` class for the WebApi response
**So that** The posting journal query results are properly mapped to HTTP response format

### Acceptance Criteria:
- [ ] Class inherits from `AccountingIdentificationDto`
- [ ] Created in `OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/`
- [ ] Has three required properties:
  - [ ] `PostingJournalTextsDto DynamicTexts` [Required]
  - [ ] `IReadOnlyCollection<StaticTextDto> StaticTexts` [Required]
  - [ ] `ValidationRuleSetDto ValidationRuleSet` [Required]
- [ ] Inherits `Number` property from base `AccountingIdentificationDto`
- [ ] Implements `Map()` method: `internal static PostingJournalResponseDto Map(PostingJournalResponse postingJournalResponse)`
  - [ ] Sets `Number` from `postingJournalResponse.PostingJournal.AccountingNumber`
  - [ ] Sets `DynamicTexts` via `PostingJournalTextsDto.Map()`
  - [ ] Sets `StaticTexts` via `StaticTextDto.Map()` for each item
  - [ ] Sets `ValidationRuleSet` via `ValidationRuleSetDto.Map()`

### Technical Details:
- No extra properties needed beyond the three core ones
- Reuses existing DTO types (all already created)
- Map method is internal and static for helper use
- Follows mapping pattern used by other DTOs in the same folder

---

## User Story 8: Create GET PostingJournal Endpoint in AccountingController

**As a** Backend Developer
**I want to** Create the GET endpoint `/api/accounting/{accountingNumber}/postingjournal` in AccountingController
**So that** React application can retrieve the posting journal for a specific accounting

### Acceptance Criteria:
- [ ] Method name: `PostingJournalAsync`
- [ ] Route: `[HttpGet("{accountingNumber:int}/postingjournal")]`
- [ ] Authorization: `[Authorize(Policy = Policies.AccountingModifier)]`
- [ ] Includes all `ProducesResponseType` attributes:
  - [ ] 200 OK: `PostingJournalResponseDto`
  - [ ] 400 Bad Request: `ProblemDetails`
  - [ ] 401 Unauthorized: `ProblemDetails`
  - [ ] 500 Internal Server Error: `ProblemDetails`
- [ ] Parameters:
  - [ ] `[FromServices] IQueryFeature<PostingJournalRequest, PostingJournalResponse> queryFeature`
  - [ ] `[FromRoute][Required][Range(...)] int accountingNumber`
  - [ ] `CancellationToken cancellationToken`
- [ ] Implementation:
  - [ ] Resolves `ISecurityContext` from `_securityContextProvider`
  - [ ] Creates `PostingJournalRequest` with today's date (via `ResolveStatusDate(null)`)
  - [ ] Executes query feature
  - [ ] Maps response via `PostingJournalResponseDto.Map()`
  - [ ] Returns `OkObjectResult`
- [ ] Using statement added: `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;`
- [ ] Follows same pattern as existing endpoints (e.g., `AccountingAsync`, `AccountingSummeryAsync`)

### Technical Details:
- StatusDate always set to today (no optional query parameter)
- Security context automatically obtained from current HTTP context
- Request ID generated as new Guid
- Format provider injected from DI
- Validation: accountingNumber must be in valid range via `[Range(...)]` attribute

---

## User Story 9: Create PostingJournalAsyncTests for Controller Endpoint

**As a** QA/Test Developer
**I want to** Create comprehensive tests for `PostingJournalAsync` controller endpoint
**So that** The WebApi endpoint correctly handles requests and responses

### Acceptance Criteria:
- [ ] Test class: `PostingJournalAsyncTests` in `OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/`
- [ ] Test: `GetCurrentSecurityContextAsync` called with correct `CancellationToken`
- [ ] Test: `ExecuteAsync` called with `PostingJournalRequest` where `RequestId` != `Guid.Empty`
- [ ] Test: `ExecuteAsync` called with correct `AccountingNumber` from route
- [ ] Test: `ExecuteAsync` called with `StatusDate` = today's date (via `ResolveStatusDate(null)`)
- [ ] Test: `ExecuteAsync` called with correct `FormatProvider`
- [ ] Test: `ExecuteAsync` called with correct `SecurityContext`
- [ ] Test: `ExecuteAsync` called with correct `CancellationToken`
- [ ] Test: Method returns `OkObjectResult`
- [ ] Test: `OkObjectResult.Value` is of type `PostingJournalResponseDto`
- [ ] Follows `AccountingAsyncTests` pattern (no optional parameters to test)
- [ ] All tests marked `[Category("UnitTest")]`
- [ ] Uses test helpers: `CreateApplyPostingJournalModel`, `CreatePostingJournalTexts`, `CreateStaticTexts`, `CreateValidationRuleSet`

### Technical Details:
- 9 test methods total
- Use NUnit 4.6.1 and Moq for mocking
- Test helpers located in:
  - `OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData.FixtureExtensions`
  - `OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos.FixtureExtensions`
  - `OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos.FixtureExtensions`

---

## User Story 10: Build Solution and Run All Tests

**As a** DevOps/QA
**I want to** Verify the solution builds successfully and all unit tests pass
**So that** The implementation is ready for integration testing and deployment

### Acceptance Criteria:
- [ ] Solution builds without errors: `dotnet build OSDevGrp.OSIntranet.Applications.sln`
- [ ] Solution builds without warnings
- [ ] All unit tests pass: `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"`
- [ ] New endpoint testable via `dotnet run` and manual HTTP requests

### Technical Details:
- No external service dependencies for unit tests
- All tests use mocks and fixtures
- Integration tests and full-stack testing deferred to manual testing phase
- Expected test count increase: ~23 new unit tests (6 + 8 + 9)

---

## User Story 11: Generate WebApi Client and Update Documentation

**As a** Backend Developer
**I want to** Trigger NSwag code generation and verify OpenAPI documentation
**So that** The new endpoint is available in the generated WebApi client and documented in OpenAPI

### Acceptance Criteria:
- [ ] Build `OSDevGrp.OSIntranet.WebApi` project to trigger post-build code generation
- [ ] Verify `WebApiClient.generated.cs` is regenerated with new endpoint
- [ ] Verify new endpoint appears in OpenAPI schema at `/openapi/v1.json`
- [ ] OpenAPI documentation shows correct route, parameters, and response types
- [ ] Generated client can be used by BFF services and external clients

### Technical Details:
- NSwag post-build triggered automatically on WebApi project build
- Generated client code in `OSDevGrp.OSIntranet.WebApi.ClientApi/` project
- Client code is packaged as NuGet package `GeneratePackageOnBuild=true`
- OpenAPI endpoint: GET /api/accounting/{accountingNumber}/postingjournal
- Response model: PostingJournalResponseDto with 3 required properties

---

## User Story 12: Update Implementation Diary with Completion Notes

**As a** Developer
**I want to** Document the completion of the PostingJournal feature implementation
**So that** Future developers understand the design decisions and implementation approach

### Acceptance Criteria:
- [ ] Diary entry updated at `docs/diary/2026-08-14-posting-journal-feature.md`
- [ ] Documents completion of all 11 user stories
- [ ] Notes any deviations from the original specification
- [ ] Includes summary of reused components (5 existing implementations)
- [ ] Documents permission model change (IsAccountingModifier vs IsAccountingViewer)
- [ ] Records lessons learned and architectural decisions

### Technical Details:
- Diary format: Markdown
- Entry date: 2026-08-14
- Include step-by-step implementation narrative
- Document the 5 reused components:
  1. `IAccountingGateway.GetPostingJournalAsync()` (ServiceGateways)
  2. `CreateApplyPostingJournalModel()` test builder (ServiceGateways.TestData)
  3. `IPostingJournalTextsBuilder` (DomainServices - already registered)
  4. `IPostingJournalRuleSetBuilder` (DomainServices - already registered)
  5. Test fixture helpers (WebApi.Tests)

---

## Implementation Summary

| Layer | Component | Type | Status | LOC* |
|-------|-----------|------|--------|-----|
| DomainServices | PostingJournalRequest | Class | New | ~10 |
| DomainServices | PostingJournalResponse | Class | New | ~18 |
| DomainServices | PostingJournalFeature | Class | New | ~52 |
| DomainServices.Tests | ExecuteAsyncTests | Class | New | ~150 |
| DomainServices.Tests | VerifyPermissionAsyncTests | Class | New | ~180 |
| WebApi | PostingJournalResponseDto | Class | New | ~25 |
| WebApi | PostingJournalAsync (endpoint) | Method | New | ~15 |
| WebApi | AccountingModifier policy | Constant + Config | New | ~15 |
| WebApi.Tests | PostingJournalAsyncTests | Class | New | ~200 |
| **Total** | | | | **~665** |

*Lines of Code (estimated)

## Reused Components

| Component | Layer | Status | Benefit |
|-----------|-------|--------|---------|
| `IAccountingGateway.GetPostingJournalAsync()` | ServiceGateways | Already Implemented | No need to create new service method |
| `CreateApplyPostingJournalModel()` | ServiceGateways.TestData | Already Implemented | Test data builders ready to use |
| `IPostingJournalTextsBuilder` | DomainServices | Already Implemented | Dynamic text building already done |
| `PostingJournalTextsBuilder` | DomainServices | Already Implemented & Registered | Registered in DI container |
| `IPostingJournalRuleSetBuilder` | DomainServices | Already Implemented & Registered | Validation rules already built |
| Test Fixture Helpers | WebApi.Tests | Already Implemented | CreatePostingJournalTexts, CreateStaticTexts, CreateValidationRuleSet |

---

## Key Architectural Decisions

1. **Permission Model:** Uses `IsAccountingModifier()` instead of `IsAccountingViewer()` because posting journal is accessed for write operations
2. **Static Text Keys:** 25 keys organized in 4 categories (headers, labels, actions, dialogs)
3. **DTO Inheritance:** PostingJournalResponseDto inherits from AccountingIdentificationDto for consistent base properties
4. **Feature Auto-Discovery:** PostingJournalFeature automatically registered via AddFeatures() mechanism - no manual DI registration needed
5. **No StatusDate Parameter:** Always uses today's date via ResolveStatusDate(null) - no query parameter variation

---

## Related Documentation

- Architecture Pattern: [AccountingIdentificationFeatureBase Pattern](../architecture/accounting-identification-feature-base.md)
- TODO List: [TODO.md](../../TODO.md)
- Implementation Diary: [2026-08-14-posting-journal-feature.md](../diary/2026-08-14-posting-journal-feature.md)
- AGENTS.md: [Build & Test Commands](../../AGENTS.md)
