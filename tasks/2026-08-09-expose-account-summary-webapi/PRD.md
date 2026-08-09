# PRD: Expose Account Summary Features via WebApi Endpoints

**Date:** 2026-08-09  
**Phase:** Phase 3 (Service Layer Integration)  
**Status:** Ready for Implementation

---

## Problem

The account summary features (AccountSummary, BudgetAccountSummary, ContactAccountSummary) were implemented in Phase 2 as domain service features with comprehensive tests. However, they are not yet exposed via REST API endpoints, blocking external clients from accessing account summary data. The WebApi layer needs to expose these three features with proper data transfer objects (DTOs), validation, error handling, and comprehensive unit tests following established controller and test patterns.

---

## Relevant Codebase

### Phase 2 Features (Implemented & Tested ✅)
- [AccountSummaryFeature.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountSummary/AccountSummaryFeature.cs)
- [BudgetAccountSummaryFeature.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/BudgetAccountSummary/BudgetAccountSummaryFeature.cs)
- [ContactAccountSummaryFeature.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/ContactAccountSummary/ContactAccountSummaryFeature.cs)
- All features auto-registered via `.AddFeatures()` assembly scan in `AddDomainServices()`

### Existing Controller Patterns (Reference)
- [AccountingController.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs)
  - Constructor dependencies: `TimeProvider`, `IFormatProvider`, `ISecurityContextProvider`
  - Existing methods: `AccountingAsync()`, `AccountingSummeryAsync()` (reference implementations)
  - Helper method: `ResolveStatusDate(statusDate)` for date normalization

### Existing DTO Patterns (Reference)
- [AccountIdentificationDto.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/AccountIdentificationDto.cs)
- [AccountInfoDto.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/AccountInfoDto.cs)
- [ValueDisplayerDto.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/ValueDisplayerDto.cs) (maps IValueDisplayer interface)
- Pattern: `required` properties with `init`, validation attributes, `internal static Map()` method

### Existing Test Patterns (Reference)
- [AccountingAsyncTests.cs](OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/AccountingAsyncTests.cs)
  - Structure: `[TestFixture]`, `[SetUp]`, AutoFixture, Moq mocks
  - Naming: `MethodName_When[Condition]_Assert[Behavior]`
  - Coverage: security context, request construction, parameter handling, response validation

### Validation & Error Handling (Existing Infrastructure)
- [ValidationValues.cs](OSDevGrp.OSIntranet.Bff.WebApi/Shared/ValidationValues.cs) — central constants for validation attributes
- [ErrorHandlerFilter.cs](OSDevGrp.OSIntranet.Bff.WebApi/Filters/ErrorHandling/ErrorHandlerFilter.cs) — global exception → ProblemDetails conversion
- [Policies.cs](OSDevGrp.OSIntranet.Bff.WebApi/Authorization/Policies.cs) — security policies including `Policies.AccountingViewer`

### Feature Registration (Existing Infrastructure)
- [ServiceCollectionExtensions.cs](OSDevGrp.OSIntranet.Bff.DomainServices/ServiceCollectionExtensions.cs)
  - `AddDomainServices()` calls `.AddFeatures()` which auto-discovers Phase 2 features
  - Phase 3 does NOT require registration changes

---

## Goal

Expose three account summary query endpoints via REST API (GET /api/accounting/{accountingNumber}/accounts/{accountNumber}/summary, etc.) with:

1. **DTOs** (6 total) — ValuesDisplayer DTOs + ResponseDtos for Account/BudgetAccount/ContactAccount
2. **Controller methods** (3 total) — AccountSummeryAsync, BudgetAccountSummeryAsync, ContactAccountSummeryAsync
3. **Validation constants** (3 total) — header MinLength values for each ValuesDisplayer type
4. **Comprehensive unit tests** (3 test files, 12+ tests each) — following AccountingAsyncTests patterns

**Success Criteria:**
- All three endpoints return 200 OK with properly mapped DTOs
- Route parameters validated (accountingNumber range, accountNumber pattern, length)
- Optional statusDate query parameter handled (null defaults to local now)
- Security context verified per request
- All tests pass; solution builds with 0 errors/warnings
- No breaking changes to existing code

---

## User Stories

### US-1: Account Summary Endpoint
**As a** client application  
**I want to** query account summary via GET `/api/accounting/{accountingNumber}/accounts/{accountNumber}/summary`  
**So that** I can retrieve credit, balance, available values at a specific status date

**Acceptance Criteria:**
- Method signature includes accountingNumber, accountNumber, optional statusDate, cancellationToken
- accountNumber validated via route constraints (length 1-16, regex pattern)
- statusDate defaults to current local date if not provided
- Response returns 200 OK with AccountSummaryResponseDto (inherits AccountInfoDto + 4 ValuesDisplayer properties)
- 12+ unit tests verify security context, request construction, statusDate handling, dependency injection, response mapping

### US-2: Budget Account Summary Endpoint
**As a** client application  
**I want to** query budget account summary via GET `/api/accounting/{accountingNumber}/budgetaccounts/{accountNumber}/summary`  
**So that** I can retrieve budget credit, balance, available values at specific periods

**Acceptance Criteria:**
- Method signature identical to Account Summary but uses BudgetAccountSummaryRequest/Response
- Response returns 200 OK with BudgetAccountSummaryResponseDto (4 ValuesDisplayer properties for different time periods)
- 12+ unit tests follow same pattern as Account Summary tests

### US-3: Contact Account Summary Endpoint
**As a** client application  
**I want to** query contact account summary via GET `/api/accounting/{accountingNumber}/contactaccounts/{accountNumber}/summary`  
**So that** I can retrieve contact credit, balance, available values at a specific status date

**Acceptance Criteria:**
- Method signature identical to Account Summary but uses ContactAccountSummaryRequest/Response
- Response returns 200 OK with ContactAccountSummaryResponseDto (3 ValuesDisplayer properties)
- 12+ unit tests follow same pattern as Account Summary tests

### US-4: Error Handling & Validation
**As a** API consumer  
**I want** proper HTTP error responses for invalid inputs  
**So that** I can handle errors gracefully

**Acceptance Criteria:**
- Route parameter validation errors return 400 BadRequest with ProblemDetails
- Missing/unauthorized security context returns 401 Unauthorized with ProblemDetails
- Unhandled exceptions caught by ErrorHandlerFilter, return 500 InternalServerError with ProblemDetails
- No custom error handling needed in controller methods (global filter handles all)

---

## Acceptance Criteria

### DTO Implementation
- [ ] `AccountValuesDisplayerDto` created in Dtos folder with Header [Required][MinLength(1)], Credit/Balance/Available ValueDisplayerDto properties
- [ ] `AccountSummaryResponseDto` created, inherits AccountInfoDto, adds StatusDate + 3 ValuesDisplayer properties with [Required]
- [ ] `BudgetAccountValuesDisplayerDto` created with identical structure to AccountValuesDisplayerDto
- [ ] `BudgetAccountSummaryResponseDto` created, inherits BudgetAccountInfoDto, adds StatusDate + 4 BudgetAccountValuesDisplayer properties
- [ ] `ContactAccountValuesDisplayerDto` created with identical structure to AccountValuesDisplayerDto
- [ ] `ContactAccountSummaryResponseDto` created, inherits ContactAccountInfoDto, adds StatusDate + 3 ContactAccountValuesDisplayer properties
- [ ] All DTOs include `internal static Map()` method converting domain model to DTO

### Controller Method Implementation
- [ ] `AccountSummeryAsync()` added with route `[HttpGet("{accountingNumber:int}/accounts/{accountNumber}/summary")]`
- [ ] `BudgetAccountSummeryAsync()` added with route `[HttpGet("{accountingNumber:int}/budgetaccounts/{accountNumber}/summary")]`
- [ ] `ContactAccountSummeryAsync()` added with route `[HttpGet("{accountingNumber:int}/contactaccounts/{accountNumber}/summary")]`
- [ ] All methods inject `IQueryFeature<Request, Response>` via `[FromServices]`
- [ ] All methods verify security context via `_securityContextProvider.GetCurrentSecurityContextAsync()`
- [ ] All methods normalize statusDate via `ResolveStatusDate()` helper
- [ ] All methods call feature.ExecuteAsync() with properly constructed request
- [ ] All methods return `Ok(ResponseDto.Map(response))`
- [ ] All methods decorated with `[Authorize(Policy = Policies.AccountingViewer)]`
- [ ] All methods include `[ProducesResponseType(...)]` for 200, 400, 401, 500 responses

### Validation Constants
- [ ] `AccountValuesDisplayerHeaderMinLength = 1` added to ValidationValues.cs
- [ ] `BudgetAccountValuesDisplayerHeaderMinLength = 1` added to ValidationValues.cs
- [ ] `ContactAccountValuesDisplayerHeaderMinLength = 1` added to ValidationValues.cs

### Unit Tests
- [ ] `AccountSummeryAsyncTests.cs` created with 12+ tests covering:
  - Security context verification (with/without statusDate)
  - Request construction (RequestId not empty, accountingNumber, accountNumber params)
  - StatusDate parameter handling (given vs. null)
  - Dependency injection (formatProvider, securityContext, cancellationToken)
  - Response type & DTO mapping
- [ ] `BudgetAccountSummeryAsyncTests.cs` created with 12+ tests (same pattern as Account)
- [ ] `ContactAccountSummeryAsyncTests.cs` created with 12+ tests (same pattern as Account)
- [ ] All tests marked `[Category("UnitTest")]`
- [ ] All tests use `[TestFixture]`, `[SetUp]`, AutoFixture, Moq patterns
- [ ] All tests follow naming convention: `MethodName_When[Condition]_Assert[Behavior]`

### Build & Validation
- [ ] Solution builds successfully: `dotnet build`
- [ ] All 2,023 existing unit tests pass
- [ ] All new unit tests pass (36+ new tests)
- [ ] No compiler errors or warnings
- [ ] No regressions in Phase 1 or Phase 2 features

---

## Scope

### In Scope
- 6 new DTO classes (ValuesDisplayer + ResponseDto for 3 account types)
- 3 new controller methods in AccountingController
- 3 validation constants in ValidationValues.cs
- 3 test files with comprehensive unit tests (12+ tests each)
- Namespace imports in AccountingController for Phase 2 feature queries
- Full integration with existing ErrorHandlerFilter, .AddFeatures(), security policies

### Out of Scope
- Phase 4 registration or service layer changes (Phase 2 features already auto-registered)
- OpenApi/Swagger documentation updates (handled by existing infrastructure)
- Integration tests or live database testing (UnitTest category only)
- React/frontend integration (WebApi responsibility complete)
- Performance optimization or caching (baseline implementation)
- Additional query parameters beyond statusDate

### Dependencies
- ✅ Phase 1 base classes (AccountIdentificationFeatureBase, etc.) — COMPLETED
- ✅ Phase 2 features (AccountSummaryFeature, BudgetAccountSummaryFeature, ContactAccountSummaryFeature) — COMPLETED
- ✅ Existing infrastructure (ErrorHandlerFilter, .AddFeatures(), validation framework) — AVAILABLE

---

## Risks & Mitigation

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|-----------|
| **DTO mapping errors** | Medium | Response data incorrect | Follow existing `ValueDisplayerDto.Map()` pattern; test all properties in acceptance tests |
| **Route parameter validation conflicts** | Low | Requests rejected unexpectedly | Use exact same `[Range]` and `[StringLength]` values as AccountingAsync reference |
| **StatusDate null-handling regression** | Low | Unexpected default dates | Test both null and provided statusDate cases; verify `ResolveStatusDate()` behavior |
| **Feature execution failures** | Low | 500 errors on valid requests | ErrorHandlerFilter will catch; ensure request construction matches feature expectations |
| **Test mock setup issues** | Medium | Tests fail to run | Replicate exact `[SetUp]` pattern from AccountingAsyncTests; use fixture seeding for reproducibility |
| **Namespace import missing** | Low | Compilation errors | Add `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.*` early in implementation |

---

## Implementation Phases

### Phase 3 Iteration 1: AccountSummery Endpoint
Create `AccountValuesDisplayerDto`, `AccountSummaryResponseDto`, `AccountSummeryAsync()` controller method, `AccountSummeryAsyncTests.cs` unit tests, and 3 validation constants.

### Phase 3 Iteration 2: BudgetAccountSummery Endpoint
Create `BudgetAccountValuesDisplayerDto`, `BudgetAccountSummaryResponseDto`, `BudgetAccountSummeryAsync()` controller method, `BudgetAccountSummeryAsyncTests.cs` unit tests.

### Phase 3 Iteration 3: ContactAccountSummery Endpoint
Create `ContactAccountValuesDisplayerDto`, `ContactAccountSummaryResponseDto`, `ContactAccountSummeryAsync()` controller method, `ContactAccountSummeryAsyncTests.cs` unit tests.

---

## Success Metrics

- ✅ 36+ new unit tests pass (12+ per iteration)
- ✅ 2,023+ total unit tests passing (2,023 existing + 36 new)
- ✅ Solution builds: 0 errors, 0 warnings
- ✅ All three endpoints respond 200 OK with correctly mapped DTOs
- ✅ All route parameters validated per specification
- ✅ All three test files follow AccountingAsyncTests patterns
- ✅ No breaking changes to Phase 1 or Phase 2 code
- ✅ Implementation diary updated with completion notes

---

## Approved By

- [x] Specification matches TODO.md Phase 3 exactly
- [x] Codebase investigation validates all patterns exist
- [x] No architectural ambiguities remain
- [x] Ready for implementation

