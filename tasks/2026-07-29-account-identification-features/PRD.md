# PRD: Account-Level Identification Features (Base Classes + Three Summary Features)

## Problem

The accounting feature layer currently supports queries at the accounting level (e.g., fetching an entire accounting with posting lines via `AccountingSummary`), but lacks a reusable foundation for account-level queries. Three account types need individual summary queries:
- **Account** (general accounts)
- **Budget Account** (budget-specific accounts)
- **Contact Account** (contact/payee/customer accounts)

Each should follow the same tested, composable pattern as `AccountingSummary` to avoid code duplication and maintain consistency. Without a base layer first, implementing these three features would repeat the same integration logic three times.

## Relevant Codebase

### Existing Pattern: AccountingSummary (Template to Follow)

**Request layer** ([AccountingSummary/AccountingSummaryRequest.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingSummary/AccountingSummaryRequest.cs)):
```csharp
public class AccountingSummaryRequest : AccountingIdentificationRequestBase
{
    public AccountingSummaryRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, 
        int numberOfPostingLines, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
        NumberOfPostingLines = numberOfPostingLines;
    }
    public int NumberOfPostingLines { get; }
}
```

**Response layer** ([AccountingSummary/AccountingSummaryResponse.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingSummary/AccountingSummaryResponse.cs)):
```csharp
public class AccountingSummaryResponse : AccountingIdentificationResponseBase<
    Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>, IAccountingTexts>
{
    public AccountingModel Accounting => Model.Item1;
    public IReadOnlyCollection<PostingLineModel> PostingLines => Model.Item2;
}
```

**Feature/orchestration layer** ([AccountingSummary/AccountingSummaryFeature.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingSummary/AccountingSummaryFeature.cs), lines 13–50):
- Calls `IAccountingGateway.GetAccountingAsync()` + `GetPostingLinesAsync()` in parallel via `Task.WhenAll()`
- Delegates validation and text building to injected builders (via `ExecuteAsync()` in base class)
- Returns response with Model, DynamicTexts, StaticTexts, ValidationRuleSet
- Returns empty dictionary in `GetStaticTextSpecifications()` (line 50)

### Base Classes That Exist Today

**AccountingIdentificationRequestBase** ([AccountingIdentificationRequestBase.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationRequestBase.cs)):
- Inherits `PageRequestBase`
- Properties: `AccountingNumber` (int), `StatusDate` (DateTimeOffset)
- Constructor: `protected AccountingIdentificationRequestBase(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)`

**AccountingIdentificationResponseBase<TModel, TDynamicTexts>** ([AccountingIdentificationResponseBase.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationResponseBase.cs)):
- Generic response class
- Constructor: `protected AccountingIdentificationResponseBase(TModel model, TDynamicTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)`
- Properties: `Model`, `DynamicTexts`, `ValidationRuleSet`

**AccountingIdentificationFeatureBase<...>** ([AccountingIdentificationFeatureBase.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationFeatureBase.cs)):
- Generic feature orchestrator
- Sealed `ExecuteAsync()` calls `GetModelAsync()` → builds texts/validation → calls `BuildResponseAsync()`
- Protected methods (for subclasses):
  - `abstract GetModelAsync(TRequest, CancellationToken): Task<TModel>`
  - `abstract BuildResponseAsync(TModel, staticTexts, dynamicTexts, validationRuleSet, CancellationToken): Task<TResponse>`
  - `abstract GetStaticTextSpecifications(TRequest, TModel): IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>`

### Gateway Interface (Source of Truth for Methods)

**IAccountingGateway** ([IAccountingGateway.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces/IAccountingGateway.cs), lines 11–13):
```csharp
Task<AccountModel> GetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);
Task<BudgetAccountModel> GetBudgetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);
Task<ContactAccountModel> GetContactAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);
```

All three methods exist and are ready to use.

### Domain Models & Text Builders Available

**Models** (from WebApi.ClientApi):
- `AccountModel` — general account data
- `BudgetAccountModel` — budget account data
- `ContactAccountModel` — contact account data

**Text builders** (interfaces in `Bff.DomainServices.Interfaces/Logic/DynamicText/`):
- `IAccountTextsBuilder : IDynamicTextsBuilder<AccountModel, IAccountTexts>`
- `IBudgetAccountTextsBuilder : IDynamicTextsBuilder<BudgetAccountModel, IBudgetAccountTexts>`
- `IContactAccountTextsBuilder : IDynamicTextsBuilder<ContactAccountModel, IContactAccountTexts>`

**Validation builder**:
- `IEmptyRuleSetBuilder : IValidationRuleSetBuilder` — for features with no validation rules

### Test Pattern: AccountingIdentificationFeatureTestBase

[AccountingIdentificationFeatureTestBase.cs](OSDevGrp.OSIntranet.Applications/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/AccountingIdentificationFeatureBase/AccountingIdentificationFeatureTestBase.cs):
- Provides static helper `CreateSut()` method with configurable mocks
- Parameterizes model getter, response builder, static text specs getter
- Returns a test-ready feature instance
- Provides `CreateAccountingIdentificationRequest()` factory

Tests using this base:
- Mock permission checker (authenticated, has access, is viewer)
- Mock static text provider
- Mock dynamic text builders
- Verify response structure, permission checks, parallel task execution

## Goal

Complete Phase 1: Create three new reusable base classes (`AccountIdentificationRequestBase`, `AccountIdentificationResponseBase`, `AccountIdentificationFeatureBase`) that mirror the accounting-level pattern but work at the account level, with full unit test coverage.

This foundation will enable Phase 2 (deferred) — implementing the three feature triplets (AccountSummary, BudgetAccountSummary, ContactAccountSummary) with minimal duplication and confidence.

## User Stories

As a **domain service developer**, I want to **query individual accounts (Account, BudgetAccount, ContactAccount) with a consistent, reusable pattern**, so that **I can avoid repeating integration logic and maintain consistency across all account types**.

As a **QA engineer**, I want to **verify that account-level base classes handle permission checks, dynamic text building, and validation consistently**, so that **all three account features work correctly without regression**.

## Acceptance Criteria

### Phase 1: Base Classes & Tests (This Work)

1. **AccountIdentificationRequestBase class exists and is correct:**
   - Path: `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountIdentificationRequestBase.cs`
   - Public, abstract
   - Inherits directly from `AccountingIdentificationRequestBase`
   - Has `AccountNumber` (string) property
   - `AccountNumber` is set by constructor (after calling base constructor)
   - Can be instantiated by a concrete subclass and used in feature tests

2. **AccountIdentificationResponseBase class exists and is correct:**
   - Path: `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountIdentificationResponseBase.cs`
   - Public, abstract, generic: `<TModel, TDynamicTexts>`
   - Inherits directly from `AccountingIdentificationResponseBase<TModel, TDynamicTexts>`
   - Constructor signature matches parent exactly (no additional parameters)
   - Can be instantiated by a concrete subclass in tests

3. **AccountIdentificationFeatureBase class exists and is correct:**
   - Path: `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountIdentificationFeatureBase.cs`
   - Internal, abstract, generic with 6 type parameters: `<TAccountIdentificationRequest, TAccountIdentificationResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder>`
   - Type constraints:
     - `TAccountIdentificationRequest : AccountIdentificationRequestBase`
     - `TAccountIdentificationResponse : AccountIdentificationResponseBase<TModel, TDynamicTexts>`
     - `TModel : class`
     - `TDynamicTexts : IDynamicTexts`
     - `TDynamicTextsBuilder : IDynamicTextsBuilder<TModel, TDynamicTexts>`
     - `TValidationRuleSetBuilder : IValidationRuleSetBuilder`
   - Inherits from `AccountingIdentificationFeatureBase<TAccountIdentificationRequest, TAccountIdentificationResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder>`
   - Constructor injects `IPermissionChecker`, `IAccountingGateway`, `IStaticTextProvider`, `TDynamicTextsBuilder`, `TValidationRuleSetBuilder`
   - Defines abstract methods (inherited from parent, no overrides needed unless requirements change):
     - `protected abstract Task<TModel> GetModelAsync(TAccountIdentificationRequest request, CancellationToken cancellationToken)`
     - `protected abstract Task<TAccountIdentificationResponse> BuildResponseAsync(TModel model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, TDynamicTexts dynamicTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)`
     - `protected abstract IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(TAccountIdentificationRequest request, TModel model)`
   - Sealed `ExecuteAsync()` orchestrates permission check, model load, text building, response construction (inherited behavior, no override)

4. **Unit tests exist for AccountIdentificationFeatureBase:**
   - Path: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/AccountIdentificationFeatureBase/AccountIdentificationFeatureTestBase.cs`
   - Public abstract test base class following the pattern of `AccountingIdentificationFeatureTestBase`
   - Provides static `CreateSut()` method with parameter variations (mocks, flags, behavior customization)
   - Provides static factory `CreateAccountIdentificationRequest()` to build test requests
   - All tests pass when run in isolation (Category=UnitTest)

5. **Code review checklist:**
   - All three base classes compile without errors
   - All #region blocks present (Constructor, Properties, Methods)
   - All classes follow codebase naming and structure conventions
   - Test bases follow existing test patterns (Fixture, Random, mocks)
   - Feature base correctly chains to accounting base without breaking encapsulation
   - No duplicate logic from accounting layer

### Phase 2: Deferred (Out of Scope for This Work)

- Implement `AccountSummaryRequest`, `AccountSummaryResponse`, `AccountSummaryFeature`
- Implement `BudgetAccountSummaryRequest`, `BudgetAccountSummaryResponse`, `BudgetAccountSummaryFeature`
- Implement `ContactAccountSummaryRequest`, `ContactAccountSummaryResponse`, `ContactAccountSummaryFeature`
- Register features in service layer
- Expose endpoints in BFF WebApi

## Scope

### In Scope

- Create three base classes (Request, Response, Feature) in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/`
- Create test base classes in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/AccountIdentificationFeatureBase/`
- All code must compile and pass unit tests
- Follow all existing codebase patterns: namespace structure, #region organization, null guards, exception handling, async patterns
- Reuse existing gateway methods and builders (no new service gateway methods needed)

### Out of Scope

- **Service layer integration**: Do not register or inject the three new features in any service layer. They are not ready.
- **WebApi controller endpoints**: No controller changes; the features are not yet plugged in.
- **Feature implementations**: AccountSummary, BudgetAccountSummary, ContactAccountSummary—these come in Phase 2.
- **Posting journal work**: Separate from this task (tracked separately).

## Risks

1. **Generic constraint complexity**: Six generic type parameters in `AccountIdentificationFeatureBase` mirrors accounting layer exactly, but if a parameter is constrained incorrectly, the feature layer won't be able to instantiate concrete features. Mitigation: Test the test base thoroughly with a concrete feature skeleton.

2. **Base class inheritance chain**: `AccountIdentificationFeatureBase` inherits from the accounting-level base, which means method signatures, async patterns, and task orchestration must align perfectly. Mismatch breaks the entire three-feature chain. Mitigation: Follow `AccountingSummary` as the reference example line-by-line.

3. **Request constructor signature**: The new `AccountIdentificationRequestBase` must inherit `AccountingNumber` and `StatusDate` from parent AND add `AccountNumber`. The constructor order matters for subclasses. Test with `AccountSummaryRequest` skeleton to verify. Mitigation: Write the skeleton and ensure it compiles before finalizing.

4. **Type parameter mismatch in concrete features** (Phase 2 risk, but affects this work's correctness): If the constraints in `AccountIdentificationFeatureBase` are too loose or too strict, the three concrete features won't compile. Example: if `TDynamicTexts` doesn't properly constrain to the builder type, reflection or DI will fail at runtime. Mitigation: Concrete test case in Phase 2 discovery.

5. **Test data: Mock builders vs. real models**: Tests will need to mock `AccountModel`, `BudgetAccountModel`, `ContactAccountModel` returned from the gateway. The TODO says "Use existing Mock for AccountModel, BudgetAccountModel and ContactAccountModel"—check whether test helpers or AutoFixture fixtures already exist. Mitigation: Search the codebase for existing mocks before writing new ones.

## Implementation Notes for Phase 1

1. **File organization**: Each base class in its own file at the top level of `Features/Queries/Accounting/` (mirroring `AccountingIdentificationRequestBase.cs`, etc. that already exist).

2. **Null guards**: Use `NullGuard.NotNull(param, nameof(param))` per codebase convention (Core.Interfaces).

3. **Async patterns**: Match the `Task.WhenAll()` pattern used in `AccountingSummaryFeature.GetModelAsync()`.

4. **Testing approach**: Create test base class first; write a minimal concrete feature skeleton to verify it compiles; then finalize test helpers and test cases.

5. **No constructor logic duplication**: Don't repeat permission checks or gateway calls in the new feature base—let the accounting base handle those; the account base just narrows constraints and adds account-level semantics.
