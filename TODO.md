# Appending posting journal for an accounting

## General

We need to append functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* OSDevGrp.OSIntranet.Bff.ServiceGateways
* OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices
* OSDevGrp.OSIntranet.Bff.WebApi

We need to create tests and test data for functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* OSDevGrp.OSIntranet.Bff.WebApi.Tests

⚠️ **Service Layer Integration**: Do not register or call the three new feature classes (AccountSummary, BudgetAccountSummary, ContactAccountSummary) in the service layer yet. They must wait until all base classes and features are completed and tested.

## Shared summary functionality

✅ **COMPLETED - Phase 1**: The following base classes and test infrastructure have been implemented (2026-08-05).

* ✅ **AccountIdentificationRequestBase** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting
  * ✅ Inherits directly from AccountingIdentificationRequestBase
  * ✅ Has AccountNumber (string) property
  * ✅ AccountNumber property set by constructor
  * **File**: AccountIdentificationRequestBase.cs

* ✅ **AccountIdentificationResponseBase** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting
  * ✅ Inherits directly from AccountingIdentificationResponseBase
  * ✅ Generic parameters: <TModel, TDynamicTexts>
  * **File**: AccountIdentificationResponseBase.cs

* ✅ **AccountIdentificationFeatureBase** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting
  * ✅ Inherits directly from AccountingIdentificationFeatureBase
  * ✅ 6 generic type parameters with correct constraints:
    - TAccountIdentificationRequest : AccountIdentificationRequestBase
    - TAccountIdentificationResponse : AccountIdentificationResponseBase<TModel, TDynamicTexts>
    - TModel : class
    - TDynamicTexts : IDynamicTexts
    - TDynamicTextsBuilder : IDynamicTextsBuilder<TModel, TDynamicTexts>
    - TValidationRuleSetBuilder : IValidationRuleSetBuilder
  * ✅ No method overrides needed - inherits ExecuteAsync orchestration from parent
  * **File**: AccountIdentificationFeatureBase.cs

* ✅ **Test Infrastructure for AccountIdentificationFeatureBase**
  * ✅ AccountIdentificationFeatureTestBase with static factory methods
  * ✅ MyAccountIdentificationRequest concrete test class
  * ✅ MyAccountIdentificationResponse concrete test class
  * ✅ VerifyPermissionAsyncTests (9 tests covering all permission scenarios)
  * ✅ ExecuteAsyncTests (16 tests covering execution paths)
  * **Total**: 39 unit tests, all passing
  * **Files**: AccountIdentificationFeatureTestBase.cs, MyAccountIdentificationRequest.cs, MyAccountIdentificationResponse.cs, VerifyPermissionAsyncTests.cs, ExecuteAsyncTests.cs

**Phase 1 Verification**:
- ✅ Solution builds: 0 errors, 0 warnings
- ✅ All 1,902 unit tests pass (39 new tests + 1,863 existing)
- ✅ No regressions
- ✅ Implementation diary complete: docs/diary/2026-07-29-account-identification-features.md
- ✅ Git commit: "Phase 1: Implement account identification base classes with comprehensive unit tests"

**Ready for Phase 2** ✅

---

## Adding account summary feature

✅ **COMPLETED - Phase 2 Iteration 1** (2026-08-06): AccountSummary feature implementation complete.

* ✅ **AccountSummaryRequest** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary
  * ✅ Inherits directly from AccountIdentificationRequestBase
  * ✅ Pass-through constructor
  * **File**: AccountSummaryRequest.cs

* ✅ **AccountSummaryResponse** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary
  * ✅ Inherits directly from AccountIdentificationResponseBase<AccountModel, IAccountTexts>
  * ✅ Convenience property: `public AccountModel Account => Model;`
  * **File**: AccountSummaryResponse.cs

* ✅ **AccountSummaryFeature** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary
  * ✅ Inherits from AccountIdentificationFeatureBase<AccountSummaryRequest, AccountSummaryResponse, AccountModel, IAccountTexts, IAccountTextsBuilder, IEmptyRuleSetBuilder>
  * ✅ GetModelAsync: calls `IAccountingGateway.GetAccountAsync()`
  * ✅ BuildResponseAsync: instantiates AccountSummaryResponse
  * ✅ GetStaticTextSpecifications: returns AccountNumberShort and AccountName static text keys
  * **File**: AccountSummaryFeature.cs

* ✅ **Test Classes for AccountSummaryFeature**
  * ✅ VerifyPermissionAsyncTests (9 tests covering all permission scenarios)
  * ✅ ExecuteAsyncTests (32 tests: 30 orchestration + 2 parameterized integration tests)
  * **Total**: 41 new unit tests, all passing
  * **Files**: VerifyPermissionAsyncTests.cs, ExecuteAsyncTests.cs
  * **Note**: AccountSummaryFeatureTests.cs was consolidated into ExecuteAsyncTests.cs; redundant mock verification tests removed

**Phase 2 Iteration 1 Verification**:
- ✅ Solution builds: 0 errors, 0 warnings
- ✅ 41 new tests pass (all AccountSummary tests)
- ✅ 1,943 total unit tests pass (up from 1,902)
- ✅ No regressions
- ✅ Feature auto-registered via `.AddFeatures()` assembly scan
- ✅ Implementation diary complete: docs/diary/2026-07-29-account-identification-features.md#step-4
- ✅ Git commit: `cd0741c344d07db9a82ceee1aafa99b98dd401d8` — "Phase 2 Iteration 1: Implement AccountSummary feature with comprehensive tests"
- ✅ Ready for Phase 2 Iterations 2 & 3 (BudgetAccountSummary, ContactAccountSummary)

## Adding budget account summary feature

✅ **COMPLETED - Phase 2 Iteration 2** (2026-08-07): BudgetAccountSummary feature implementation complete.

* ✅ **BudgetAccountSummaryRequest** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary
  * ✅ Inherits directly from AccountIdentificationRequestBase
  * ✅ Pass-through constructor
  * **File**: BudgetAccountSummaryRequest.cs

* ✅ **BudgetAccountSummaryResponse** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary
  * ✅ Inherits directly from AccountIdentificationResponseBase<BudgetAccountModel, IBudgetAccountTexts>
  * ✅ Convenience property: `public BudgetAccountModel BudgetAccount => Model;`
  * **File**: BudgetAccountSummaryResponse.cs

* ✅ **BudgetAccountSummaryFeature** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary
  * ✅ Inherits from AccountIdentificationFeatureBase<BudgetAccountSummaryRequest, BudgetAccountSummaryResponse, BudgetAccountModel, IBudgetAccountTexts, IBudgetAccountTextsBuilder, IEmptyRuleSetBuilder>
  * ✅ GetModelAsync: calls `IAccountingGateway.GetBudgetAccountAsync()`
  * ✅ BuildResponseAsync: instantiates BudgetAccountSummaryResponse
  * ✅ GetStaticTextSpecifications: returns AccountNumberShort and AccountName static text keys
  * **File**: BudgetAccountSummaryFeature.cs

* ✅ **Test Classes for BudgetAccountSummaryFeature**
  * ✅ VerifyPermissionAsyncTests (9 tests covering all permission scenarios)
  * ✅ ExecuteAsyncTests (32 tests: 30 orchestration + 2 parameterized integration tests)
  * **Total**: 41 new unit tests, all passing
  * **Files**: VerifyPermissionAsyncTests.cs, ExecuteAsyncTests.cs

**Phase 2 Iteration 2 Verification**:
- ✅ Solution builds: 0 errors, 0 warnings
- ✅ 41 new tests pass (all BudgetAccountSummary tests)
- ✅ 2,023 total unit tests pass (up from 1,943)
- ✅ No regressions
- ✅ Feature auto-registered via `.AddFeatures()` assembly scan
- ✅ Implementation diary updated: docs/diary/2026-07-29-account-identification-features.md
- ✅ Git commit: `ecd3e939e2764442e6fc047dc46fb0bc22bc43f4` — "Phase 2 Iterations 2 & 3: Implement BudgetAccountSummary and ContactAccountSummary features with comprehensive tests"
- ✅ Ready for Phase 3 (Service Layer Integration)

## Adding contact account summary feature

✅ **COMPLETED - Phase 2 Iteration 3** (2026-08-07): ContactAccountSummary feature implementation complete.

* ✅ **ContactAccountSummaryRequest** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary
  * ✅ Inherits directly from AccountIdentificationRequestBase
  * ✅ Pass-through constructor
  * **File**: ContactAccountSummaryRequest.cs

* ✅ **ContactAccountSummaryResponse** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary
  * ✅ Inherits directly from AccountIdentificationResponseBase<ContactAccountModel, IContactAccountTexts>
  * ✅ Convenience property: `public ContactAccountModel ContactAccount => Model;`
  * **File**: ContactAccountSummaryResponse.cs

* ✅ **ContactAccountSummaryFeature** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary
  * ✅ Inherits from AccountIdentificationFeatureBase<ContactAccountSummaryRequest, ContactAccountSummaryResponse, ContactAccountModel, IContactAccountTexts, IContactAccountTextsBuilder, IEmptyRuleSetBuilder>
  * ✅ GetModelAsync: calls `IAccountingGateway.GetContactAccountAsync()`
  * ✅ BuildResponseAsync: instantiates ContactAccountSummaryResponse
  * ✅ GetStaticTextSpecifications: returns AccountNumberShort and AccountName static text keys
  * **File**: ContactAccountSummaryFeature.cs

* ✅ **Test Classes for ContactAccountSummaryFeature**
  * ✅ VerifyPermissionAsyncTests (9 tests covering all permission scenarios)
  * ✅ ExecuteAsyncTests (32 tests: 30 orchestration + 2 parameterized integration tests)
  * **Total**: 41 new unit tests, all passing
  * **Files**: VerifyPermissionAsyncTests.cs, ExecuteAsyncTests.cs

**Phase 2 Iteration 3 Verification**:
- ✅ Solution builds: 0 errors, 0 warnings
- ✅ 41 new tests pass (all ContactAccountSummary tests)
- ✅ 2,023 total unit tests pass (up from 1,943)
- ✅ No regressions
- ✅ Feature auto-registered via `.AddFeatures()` assembly scan
- ✅ Implementation diary updated: docs/diary/2026-07-29-account-identification-features.md
- ✅ Git commit: `ecd3e939e2764442e6fc047dc46fb0bc22bc43f4` — "Phase 2 Iterations 2 & 3: Implement BudgetAccountSummary and ContactAccountSummary features with comprehensive tests"
- ✅ Ready for Phase 3 (Service Layer Integration)

---

## Summary: Phase 2 Complete ✅

**All three account summary features are now implemented and tested:**
- ✅ AccountSummary (Phase 2.1) — 41 tests
- ✅ BudgetAccountSummary (Phase 2.2) — 41 tests
- ✅ ContactAccountSummary (Phase 2.3) — 41 tests

**Total new tests**: 123 unit tests added (1,943 → 2,023)

**Next phase**: Phase 3 — Service Layer Integration (expose account summary features via WebApi endpoints with DTOs)

---

## Phase 3: Expose Account Summary Features via WebApi

**Architecture Notes**:
* **Error Handling**: Global exception handling via `ErrorHandlerFilter` converts exceptions to standardized ProblemDetails responses (400 BadRequest, 401 Unauthorized, 500 InternalServerError). No custom error handling needed in controllers.
* **Feature Auto-Registration**: Phase 2 features (AccountSummaryFeature, BudgetAccountSummaryFeature, ContactAccountSummaryFeature) are auto-discovered and registered by `AddDomainServices()` via `.AddFeatures()` assembly scan. No Phase 4 registration step needed.
* **Security Policy**: `[Authorize(Policy = Policies.AccountingViewer)]` uses existing policy already configured in the application.
* **DTO Naming**: `AccountingSummaryResponseDto` (broader accounting summary) and `AccountSummaryResponseDto` (specific account summary) are intentionally different classes serving different purposes.

**ValidationValues.cs** (Shared/ValidationValues.cs):
* New constants to be added:
  * `internal const int AccountValuesDisplayerHeaderMinLength = 1;`
  * `internal const int BudgetAccountValuesDisplayerHeaderMinLength = 1;`
  * `internal const int ContactAccountValuesDisplayerHeaderMinLength = 1;`
* Status: DOCUMENTED in TODO.md, NOT YET IMPLEMENTED

### Adding AccountSummeryAsync endpoint

⚠️ **In Progress**: Implement `AccountSummeryAsync` method in `AccountingController` in OSDevGrp.OSIntranet.Bff.WebApi

**Note**: This method is added to the existing `AccountingController` class in `OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs` (not a new file). The controller already has required dependencies: `_securityContextProvider`, `_formatProvider`, and `ResolveStatusDate(statusDate)` helper method.

**Controller Implementation**:
* Add required namespace imports:
  * `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary;`
* Decorator: `[Authorize(Policy = Policies.AccountingViewer)]`
* HTTP route: `[HttpGet("{accountingNumber:int}/accounts/{accountNumber}/summary")]` (endpoint pattern: `/api/accounting/{accountingNumber}/accounts/{accountNumber}/summary`)
* Method signature should accept:
  * `[FromServices] IQueryFeature<AccountSummaryRequest, AccountSummaryResponse> queryFeature` — injected feature for executing the query
  * `[FromRoute][Required][Range(AccountingRuleSetSpecifications.AccountingNumberMinValue, AccountingRuleSetSpecifications.AccountingNumberMaxValue)] int accountingNumber` — accounting number from route
  * `[FromRoute][Required][StringLength(AccountingRuleSetSpecifications.AccountNumberMaxLength, MinimumLength = AccountingRuleSetSpecifications.AccountNumberMinLength)][RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)] string accountNumber` — account number from route
  * `CancellationToken cancellationToken` — cancellation token
  * `[FromQuery] DateTimeOffset? statusDate = null` — optional query parameter for status date
* Method implementation:
  * Get current security context via `_securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken)`
  * Create `AccountSummaryRequest` with: `Guid.NewGuid()`, `accountingNumber`, `accountNumber`, `ResolveStatusDate(statusDate)`, `_formatProvider`, `securityContext`
  * Execute feature: `queryFeature.ExecuteAsync(accountSummaryRequest, cancellationToken)`
  * Map response to DTO: `AccountSummaryResponseDto.Map(accountSummaryResponse)`
  * Return: `Ok(...)` with mapped response
* Response types (ProducesResponseType):
  * 200 OK: `AccountSummaryResponseDto`
  * 400 BadRequest: `ProblemDetails`
  * 401 Unauthorized: `ProblemDetails`
  * 500 InternalServerError: `ProblemDetails`

**DTO Implementation**:
* Create `AccountValuesDisplayerDto` in `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`
  * Maps the `IAccountValuesDisplayer` interface
  * Properties (all required since they are non-nullable in the interface):
    * `[Required][MinLength(ValidationValues.AccountValuesDisplayerHeaderMinLength)] string Header` — required, represents the header for the values display
    * `[Required] ValueDisplayerDto Credit` — required, represents credit value (maps `IValueDisplayer`)
    * `[Required] ValueDisplayerDto Balance` — required, represents balance value (maps `IValueDisplayer`)
    * `[Required] ValueDisplayerDto Available` — required, represents available value (maps `IValueDisplayer`)
  * Implement `Map()` static method to convert `IAccountValuesDisplayer` to `AccountValuesDisplayerDto`

* Create `AccountSummaryResponseDto` in `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`
  * Should inherit from `AccountInfoDto` (inherits base properties: AccountNumber, AccountName)
  * Include `[Required] ValueDisplayerDto StatusDate` — required
  * Include `[Required] AccountValuesDisplayerDto ValuesAtStatusDate` — required (from `IAccountValuesDisplayer`)
  * Include `[Required] AccountValuesDisplayerDto ValuesAtEndOfLastMonthFromStatusDate` — required (from `IAccountValuesDisplayer`)
  * Include `[Required] AccountValuesDisplayerDto ValuesAtEndOfLastYearFromStatusDate` — required (from `IAccountValuesDisplayer`)
  * Implement `Map()` static method to convert `AccountSummaryResponse` to `AccountSummaryResponseDto`

**Test Implementation**:
* Create unit test file: `OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/AccountSummeryAsyncTests.cs`
  * **Test Class**: `AccountSummeryAsyncTests` covering the `AccountSummeryAsync` controller method (following exact pattern from `AccountingAsyncTests`)
  * **Test Setup**: 
    * Mock dependencies: `TimeProvider`, `ISecurityContextProvider`, `IQueryFeature<AccountSummaryRequest, AccountSummaryResponse>`
    * Use `Fixture` and `Random` pattern (AutoFixture, NUnit [TestFixture], [SetUp])
  * **Security Context Tests**:
    * Test: `AccountSummeryAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken` (parameterized with/without statusDate)
  * **Request Construction Tests**:
    * Test: `AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereRequestIdIsNotEqualToGuidEmpty` (parameterized)
    * Test: `AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereAccountingNumberIsEqualToGivenAccountingNumber` (parameterized)
    * Test: `AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereAccountNumberIsEqualToGivenAccountNumber` (NEW: route parameter)
  * **StatusDate Parameter Tests**:
    * Test: `AccountSummeryAsync_WhenStatusDateIsGiven_AssertGetUtcNowWasNotCalledOnTimeProvider`
    * Test: `AccountSummeryAsync_WhenStatusDateIsGiven_AssertLocalTimeZoneWasNotCalledOnTimeProvider`
    * Test: `AccountSummeryAsync_WhenStatusDateIsGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereStatusDateIsEqualToGivenStatusDate`
    * Test: `AccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertGetUtcNowWasCalledOnTimeProvider`
    * Test: `AccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertLocalTimeZoneWasCalledOnTimeProvider`
    * Test: `AccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider`
  * **Dependency Injection Tests**:
    * Test: `AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies` (parameterized)
    * Test: `AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider` (parameterized)
    * Test: `AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken` (parameterized)
  * **Response Tests**:
    * Test: `AccountSummeryAsync_WhenCalled_ReturnsOkObjectResult` (parameterized)
    * Test: `AccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsAccountSummaryResponseDto` (parameterized)
    * Test: `AccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsAccountSummaryResponseDtoWithAllPropertiesCorrectlyMapped` — verifies DTO mapping via `AccountSummaryResponseDto.Map()` returns correctly typed object with all properties (AccountNumber, AccountName, StatusDate, ValuesAtStatusDate, ValuesAtEndOfLastMonthFromStatusDate, ValuesAtEndOfLastYearFromStatusDate)
  * **Test Categories**: All tests marked with `[Category("UnitTest")]` and use `[TestCase]` or `[TestFixture]` patterns

---

## Phase 3 Iteration 2: Adding BudgetAccountSummeryAsync endpoint

⏳ **In Progress**: Implement `BudgetAccountSummeryAsync` method in `AccountingController` in OSDevGrp.OSIntranet.Bff.WebApi

**Note**: This method is added to the existing `AccountingController` class in `OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs` (not a new file). The controller already has required dependencies: `_securityContextProvider`, `_formatProvider`, and `ResolveStatusDate(statusDate)` helper method.

**Controller Implementation**:
* Add required namespace imports:
  * `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary;`
* Decorator: `[Authorize(Policy = Policies.AccountingViewer)]`
* HTTP route: `[HttpGet("{accountingNumber:int}/budgetaccounts/{accountNumber}/summary")]` (endpoint pattern: `/api/accounting/{accountingNumber}/budgetaccounts/{accountNumber}/summary`)
* Method signature: Same parameter pattern as AccountSummeryAsync but with `IQueryFeature<BudgetAccountSummaryRequest, BudgetAccountSummaryResponse>`
* Method implementation: Same logic pattern as AccountSummeryAsync but using BudgetAccountSummaryRequest/Response and BudgetAccountSummaryResponseDto
* Response types (ProducesResponseType): Same as AccountSummeryAsync (200 OK, 400 BadRequest, 401 Unauthorized, 500 InternalServerError)

**DTO Implementation**:
* Create `BudgetAccountValuesDisplayerDto` in `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`
  * Maps the `IBudgetAccountValuesDisplayer` interface
  * Properties (all required since they are non-nullable in the interface):
    * `[Required][MinLength(ValidationValues.BudgetAccountValuesDisplayerHeaderMinLength)] string Header` — required, represents the header for the values display
    * `[Required] ValueDisplayerDto Credit` — required, represents credit value (maps `IValueDisplayer`)
    * `[Required] ValueDisplayerDto Balance` — required, represents balance value (maps `IValueDisplayer`)
    * `[Required] ValueDisplayerDto Available` — required, represents available value (maps `IValueDisplayer`)
  * Implement `Map()` static method to convert `IBudgetAccountValuesDisplayer` to `BudgetAccountValuesDisplayerDto`
* Create `BudgetAccountSummaryResponseDto` in `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`
  * Should inherit from `BudgetAccountInfoDto` (inherits base properties: AccountNumber, AccountName)
  * Include `[Required] ValueDisplayerDto StatusDate` — required
  * Include `[Required] BudgetAccountValuesDisplayerDto ValuesForMonthOfStatusDate` — required (from `IBudgetAccountValuesDisplayer`)
  * Include `[Required] BudgetAccountValuesDisplayerDto ValuesForLastMonthOfStatusDate` — required (from `IBudgetAccountValuesDisplayer`)
  * Include `[Required] BudgetAccountValuesDisplayerDto ValuesForYearToDateOfStatusDate` — required (from `IBudgetAccountValuesDisplayer`)
  * Include `[Required] BudgetAccountValuesDisplayerDto ValuesForLastYearOfStatusDate` — required (from `IBudgetAccountValuesDisplayer`)
  * Implement `Map()` static method to convert `BudgetAccountSummaryResponse` to `BudgetAccountSummaryResponseDto`

**Test Implementation**:
* Create unit test file: `OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/BudgetAccountSummeryAsyncTests.cs`
  * **Test Class**: `BudgetAccountSummeryAsyncTests` covering the `BudgetAccountSummeryAsync` controller method (following exact pattern from `AccountingAsyncTests`)
  * **Test Setup**: 
    * Mock dependencies: `TimeProvider`, `ISecurityContextProvider`, `IQueryFeature<BudgetAccountSummaryRequest, BudgetAccountSummaryResponse>`
    * Use `Fixture` and `Random` pattern (AutoFixture, NUnit [TestFixture], [SetUp])
  * **Security Context Tests**:
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken` (parameterized with/without statusDate)
  * **Request Construction Tests**:
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereRequestIdIsNotEqualToGuidEmpty` (parameterized)
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereAccountingNumberIsEqualToGivenAccountingNumber` (parameterized)
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereAccountNumberIsEqualToGivenAccountNumber` (route parameter — note: still called `accountNumber`)
  * **StatusDate Parameter Tests**:
    * Test: `BudgetAccountSummeryAsync_WhenStatusDateIsGiven_AssertGetUtcNowWasNotCalledOnTimeProvider`
    * Test: `BudgetAccountSummeryAsync_WhenStatusDateIsGiven_AssertLocalTimeZoneWasNotCalledOnTimeProvider`
    * Test: `BudgetAccountSummeryAsync_WhenStatusDateIsGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereStatusDateIsEqualToGivenStatusDate`
    * Test: `BudgetAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertGetUtcNowWasCalledOnTimeProvider`
    * Test: `BudgetAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertLocalTimeZoneWasCalledOnTimeProvider`
    * Test: `BudgetAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider`
  * **Dependency Injection Tests**:
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies` (parameterized)
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithBudgetAccountSummaryRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider` (parameterized)
    * Test: `BudgetAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken` (parameterized)
  * **Response Tests**:
    * Test: `BudgetAccountSummeryAsync_WhenCalled_ReturnsOkObjectResult` (parameterized)
    * Test: `BudgetAccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsBudgetAccountSummaryResponseDto` (parameterized)
    * Test: `BudgetAccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsBudgetAccountSummaryResponseDtoWithAllPropertiesCorrectlyMapped` — verifies DTO mapping via `BudgetAccountSummaryResponseDto.Map()` returns correctly typed object with all properties (AccountNumber, AccountName, StatusDate, ValuesForMonthOfStatusDate, ValuesForLastMonthOfStatusDate, ValuesForYearToDateOfStatusDate, ValuesForLastYearOfStatusDate)
  * **Test Categories**: All tests marked with `[Category("UnitTest")]` and use `[TestCase]` or `[TestFixture]` patterns

---

## Phase 3 Iteration 3: Adding ContactAccountSummeryAsync endpoint

⏳ **In Progress**: Implement `ContactAccountSummeryAsync` method in `AccountingController` in OSDevGrp.OSIntranet.Bff.WebApi

**Note**: This method is added to the existing `AccountingController` class in `OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs` (not a new file). The controller already has required dependencies: `_securityContextProvider`, `_formatProvider`, and `ResolveStatusDate(statusDate)` helper method.

**Controller Implementation**:
* Add required namespace imports:
  * `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary;`
* Decorator: `[Authorize(Policy = Policies.AccountingViewer)]`
* HTTP route: `[HttpGet("{accountingNumber:int}/contactaccounts/{accountNumber}/summary")]` (endpoint pattern: `/api/accounting/{accountingNumber}/contactaccounts/{accountNumber}/summary`)
* Method signature: Same parameter pattern as AccountSummeryAsync but with `IQueryFeature<ContactAccountSummaryRequest, ContactAccountSummaryResponse>`
* Method implementation: Same logic pattern as AccountSummeryAsync but using ContactAccountSummaryRequest/Response and ContactAccountSummaryResponseDto
* Response types (ProducesResponseType): Same as AccountSummeryAsync (200 OK, 400 BadRequest, 401 Unauthorized, 500 InternalServerError)

**DTO Implementation**:
* Create `ContactAccountValuesDisplayerDto` in `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`
  * Maps the `IContactAccountValuesDisplayer` interface
  * Properties (all required since they are non-nullable in the interface):
    * `[Required][MinLength(ValidationValues.ContactAccountValuesDisplayerHeaderMinLength)] string Header` — required, represents the header for the values display
    * `[Required] ValueDisplayerDto Credit` — required, represents credit value (maps `IValueDisplayer`)
    * `[Required] ValueDisplayerDto Balance` — required, represents balance value (maps `IValueDisplayer`)
    * `[Required] ValueDisplayerDto Available` — required, represents available value (maps `IValueDisplayer`)
  * Implement `Map()` static method to convert `IContactAccountValuesDisplayer` to `ContactAccountValuesDisplayerDto`
* Create `ContactAccountSummaryResponseDto` in `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`
  * Should inherit from `ContactAccountInfoDto` (inherits base properties: AccountNumber, AccountName)
  * Include `[Required] ValueDisplayerDto StatusDate` — required
  * Include `[Required] ContactAccountValuesDisplayerDto ValuesAtStatusDate` — required (from `IContactAccountValuesDisplayer`)
  * Include `[Required] ContactAccountValuesDisplayerDto ValuesAtEndOfLastMonthFromStatusDate` — required (from `IContactAccountValuesDisplayer`)
  * Include `[Required] ContactAccountValuesDisplayerDto ValuesAtEndOfLastYearFromStatusDate` — required (from `IContactAccountValuesDisplayer`)
  * Implement `Map()` static method to convert `ContactAccountSummaryResponse` to `ContactAccountSummaryResponseDto`

**Test Implementation**:
* Create unit test file: `OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/ContactAccountSummeryAsyncTests.cs`
  * **Test Class**: `ContactAccountSummeryAsyncTests` covering the `ContactAccountSummeryAsync` controller method (following exact pattern from `AccountingAsyncTests`)
  * **Test Setup**: 
    * Mock dependencies: `TimeProvider`, `ISecurityContextProvider`, `IQueryFeature<ContactAccountSummaryRequest, ContactAccountSummaryResponse>`
    * Use `Fixture` and `Random` pattern (AutoFixture, NUnit [TestFixture], [SetUp])
  * **Security Context Tests**:
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken` (parameterized with/without statusDate)
  * **Request Construction Tests**:
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereRequestIdIsNotEqualToGuidEmpty` (parameterized)
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereAccountingNumberIsEqualToGivenAccountingNumber` (parameterized)
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereAccountNumberIsEqualToGivenAccountNumber` (route parameter — note: still called `accountNumber`)
  * **StatusDate Parameter Tests**:
    * Test: `ContactAccountSummeryAsync_WhenStatusDateIsGiven_AssertGetUtcNowWasNotCalledOnTimeProvider`
    * Test: `ContactAccountSummeryAsync_WhenStatusDateIsGiven_AssertLocalTimeZoneWasNotCalledOnTimeProvider`
    * Test: `ContactAccountSummeryAsync_WhenStatusDateIsGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereStatusDateIsEqualToGivenStatusDate`
    * Test: `ContactAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertGetUtcNowWasCalledOnTimeProvider`
    * Test: `ContactAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertLocalTimeZoneWasCalledOnTimeProvider`
    * Test: `ContactAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider`
  * **Dependency Injection Tests**:
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies` (parameterized)
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider` (parameterized)
    * Test: `ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken` (parameterized)
  * **Response Tests**:
    * Test: `ContactAccountSummeryAsync_WhenCalled_ReturnsOkObjectResult` (parameterized)
    * Test: `ContactAccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsContactAccountSummaryResponseDto` (parameterized)
    * Test: `ContactAccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsContactAccountSummaryResponseDtoWithAllPropertiesCorrectlyMapped` — verifies DTO mapping via `ContactAccountSummaryResponseDto.Map()` returns correctly typed object with all properties (AccountNumber, AccountName, StatusDate, ValuesAtStatusDate, ValuesAtEndOfLastMonthFromStatusDate, ValuesAtEndOfLastYearFromStatusDate)
  * **Test Categories**: All tests marked with `[Category("UnitTest")]` and use `[TestCase]` or `[TestFixture]` patterns
