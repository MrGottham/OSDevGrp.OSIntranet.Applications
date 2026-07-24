# PRD: Extend Dynamic Texts for Accounts, Budget Accounts, and Contact Accounts

## Problem

Three marker interfaces (`IAccountTexts`, `IBudgetAccountTexts`, `IContactAccountTexts`) currently exist but provide no display functionality. The UI needs formatted, localized representations of account values across different date perspectives (status date, end of last month, end of last year for regular accounts; month/year ranges for budget accounts; simple date for contact accounts). Currently these are empty shells — implementing them will enable rich text rendering of account data following the established pattern used successfully by `AccountingTexts`.

## Relevant Codebase

### What Exists Today

**Marker Interfaces (Empty):**
- [IAccountTexts.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IAccountTexts.cs) — empty, inherits `IDynamicTexts`
- [IBudgetAccountTexts.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IBudgetAccountTexts.cs) — empty, inherits `IDynamicTexts`
- [IContactAccountTexts.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IContactAccountTexts.cs) — empty, inherits `IDynamicTexts`

**Current Implementations (Pass-through Only):**
- [AccountTexts.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountTexts.cs) — inherits `DynamicTextsBase<AccountModel>`, stores model + format provider only
- [BudgetAccountTexts.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/BudgetAccountTexts.cs) — inherits `DynamicTextsBase<BudgetAccountModel>`, pass-through only
- [ContactAccountTexts.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/ContactAccountTexts.cs) — inherits `DynamicTextsBase<ContactAccountModel>`, pass-through only

**Builders (Simple, No Displayer Construction):**
- [AccountTextsBuilder.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountTextsBuilder.cs) — builds empty `IAccountTexts` instance
- [BudgetAccountTextsBuilder.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/BudgetAccountTextsBuilder.cs) — builds empty `IBudgetAccountTexts` instance
- [ContactAccountTextsBuilder.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/ContactAccountTextsBuilder.cs) — builds empty `IContactAccountTexts` instance

**Existing Display Pattern (To Mirror):**
- [IValueDisplayer.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IValueDisplayer.cs) — interface with `Label` (string) and `Value` (string?) properties
- [ValueDisplayer.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/ValueDisplayer.cs) — implementation with generic `TValue` type parameter, lambda-based formatting
- [IBalanceSheetDisplayer.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IBalanceSheetDisplayer.cs) — example composite displayer: `Header` (string) + `Assets` / `Liabilities` (both `IValueDisplayer`)
- [BalanceSheetDisplayer.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/BalanceSheetDisplayer.cs) — internal class with private constructor + `CreateAsync()` static factory (pattern to follow)

**Base Classes & Infrastructure:**
- [DynamicTextsBase.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/DynamicTextsBase.cs) — abstract base with `Model` and `FormatProvider` properties
- [DynamicTextsBuilderBase.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/DynamicTextsBuilderBase.cs) — generic builder base with helpers:
  - `GetStatusDateAsync()` — formats a `DateTimeOffset` using static text key + format string
  - `GetValueDisplayerAsync<TValue>()` — creates `IValueDisplayer` with label from static text provider + formatted value
- [StaticTextProvider.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs) — singleton that maps `StaticTextKey` enum to Danish labels; returns localized strings via `GetStaticTextAsync()`
- [StaticTextKey.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs) — enum with 100+ entries; currently includes: `StatusDate`, `Credit`, `Balance`, `Available`, `Budget`, `Posted` (these will be reused)

**Models (Data Source):**
- [AccountModel](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs) (auto-generated):
  - `StatusDate` (DateTimeOffset)
  - `ValuesAtStatusDate` (CreditInfoValuesModel with `Credit`, `Balance`, `Available` decimal properties)
  - `ValuesAtEndOfLastMonthFromStatusDate` (CreditInfoValuesModel)
  - `ValuesAtEndOfLastYearFromStatusDate` (CreditInfoValuesModel)
- [BudgetAccountModel](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs) (auto-generated):
  - `StatusDate` (DateTimeOffset)
  - `ValuesForMonthOfStatusDate` (BudgetInfoValuesModel with `Budget`, `Posted`, `Available` decimal properties)
  - `ValuesForLastMonthOfStatusDate` (BudgetInfoValuesModel)
  - `ValuesForYearToDateOfStatusDate` (BudgetInfoValuesModel)
  - `ValuesForLastYearOfStatusDate` (BudgetInfoValuesModel)
- [ContactAccountModel](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs) (auto-generated):
  - `StatusDate` (DateTimeOffset)
  - `ValuesAtStatusDate` (BalanceInfoValuesModel with `Balance` decimal property)
  - `ValuesAtEndOfLastMonthFromStatusDate` (BalanceInfoValuesModel)
  - `ValuesAtEndOfLastYearFromStatusDate` (BalanceInfoValuesModel)

**Existing Tests (Pattern to Mirror):**
- [AccountingTextsBuilderTests.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountingTextsBuilderTests.cs) — ~200 lines of comprehensive tests covering setup, mocked displayer construction, parallel task verification, null assertions
- [BalanceSheetDisplayerTests.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/BalanceSheetDisplayerTests.cs) — tests for `CreateAsync()` factory method: parameter validation, displayer property verification

**DI Registration:**
- [ServiceCollectionExtensions.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/ServiceCollectionExtensions.cs) lines 35–40 — already registers `IAccountTextsBuilder`, `IBudgetAccountTextsBuilder`, `IContactAccountTextsBuilder` as transient

**Data Flow (How It Works Today):**
1. `StaticTextProvider` (singleton) stores map of `StaticTextKey` → Danish label string
2. `DynamicTextsBuilderBase` provides helper methods that call `StaticTextProvider.GetStaticTextAsync()` to fetch localized labels
3. Displayer classes (like `BalanceSheetDisplayer`) use `CreateAsync()` static factory to construct instances, formatting values with lambdas like `(v, fp) => v.ToString("C", fp)`
4. Text classes (like `AccountTexts`) store collections of displayers as properties, exposing them to the UI layer
5. Builders orchestrate displayer creation in parallel using `Task.ContinueWith()` + `Task.WhenAll()`, then construct the text object

### Patterns to Follow

**Composite Displayer Pattern:**
```csharp
// Interface: groups related IValueDisplayer objects + a Header string
public interface IBalanceSheetDisplayer
{
    string Header { get; }
    IValueDisplayer Assets { get; }
    IValueDisplayer Liabilities { get; }
}

// Implementation: private constructor + static CreateAsync factory
internal class BalanceSheetDisplayer : IBalanceSheetDisplayer
{
    private BalanceSheetDisplayer(string header, IValueDisplayer assets, IValueDisplayer liabilities) { … }
    
    internal static async Task<IBalanceSheetDisplayer> CreateAsync<TModel>(
        StaticTextKey headerKey, StaticTextKey assetsKey, StaticTextKey liabilitiesKey,
        IStaticTextProvider staticTextProvider, TModel model, 
        Func<TModel, decimal> assetsCalculator, Func<TModel, decimal> liabilitiesCalculator,
        IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        var headerText = await staticTextProvider.GetStaticTextAsync(…);
        var assetsLabel = await staticTextProvider.GetStaticTextAsync(…);
        var liabilitiesLabel = await staticTextProvider.GetStaticTextAsync(…);
        
        return new BalanceSheetDisplayer(
            headerText,
            new ValueDisplayer<decimal>(assetsLabel, assetsCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(liabilitiesLabel, liabilitiesCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)));
    }
}
```

**Builder Pattern (Parallel Construction):**
```csharp
// From AccountingTextsBuilder.BuildAsync():
internal override async Task<IAccountingTexts> BuildAsync(…)
{
    // Declare nullable variables for each displayer
    IValueDisplayer? statusDate = null;
    IBalanceSheetDisplayer? balanceSheet = null;
    // … more displayers …
    
    // Build all in parallel using Task.ContinueWith()
    Task buildStatusDateTask = GetStatusDateAsync(…).ContinueWith(task => statusDate = task.Result);
    Task buildBalanceSheetTask = BalanceSheetDisplayer.CreateAsync(…).ContinueWith(task => balanceSheet = task.Result);
    // … more tasks …
    
    await Task.WhenAll(buildStatusDateTask, buildBalanceSheetTask, /* … */);
    
    // Construct text object with all non-null displayers
    return new AccountingTexts(model, statusDate!, balanceSheet!, /* … */, formatProvider);
}
```

**Test Pattern (From AccountingTextsBuilderTests):**
- Setup: Create fixture, mock `IStaticTextProvider`, mock models
- Test each displayer construction: verify correct static text keys fetched, verify calculations applied
- Test builder.BuildAsync(): verify all displayers instantiated and assigned to text object properties
- Test null handling: verify displayers are properly awaited and not null
- Test exception propagation: mock provider throwing, verify exception bubbles up

## Goal

Make `IAccountTexts`, `IBudgetAccountTexts`, and `IContactAccountTexts` rich display objects that wrap account/budget/contact values in localized, formatted labels. Enable UI to render account data across multiple date perspectives without custom formatting logic. Each text object will expose displayers (grouped by date perspective) composed of `IValueDisplayer` instances with labels + formatted currency values.

## User Stories

**As a UI developer**, I want to display account values (credit, balance, available) at multiple date snapshots (status date, end of last month, end of last year) with localized labels and formatted currency, so that I can render account details without writing formatting logic in the component layer.

**As a UI developer**, I want to display budget account values (budget, posted, available) for multiple date ranges (month of status date, last month, year-to-date, last year) with localized labels and formatted currency, so that I can compare budget vs. actual spending across time periods without custom formatting.

**As a UI developer**, I want to display contact account balances at multiple date snapshots with localized labels and formatted currency, so that I can show historical balance information with minimal component logic.

**As a developer**, I want the three new text-building extensions to follow the same builder pattern as `AccountingTexts`, so that the codebase remains consistent and maintainable.

**As a developer**, I want comprehensive unit tests for all new displayers and builders, so that refactoring and future changes don't break account data formatting.

## Acceptance Criteria

### Phase 1: Accounts (IAccountTexts, IAccountValuesDisplayer)

1. **New interface `IAccountValuesDisplayer`** exists at [OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IAccountValuesDisplayer.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IAccountValuesDisplayer.cs) with read-only properties:
   - `Header` (string)
   - `Credit` (IValueDisplayer)
   - `Balance` (IValueDisplayer)
   - `Available` (IValueDisplayer)

2. **New class `AccountValuesDisplayer`** (internal) exists at [OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountValuesDisplayer.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountValuesDisplayer.cs) with:
   - Private constructor taking all 4 properties
   - Static `CreateAsync(StaticTextKey headerKey, CreditInfoValuesModel values, IStaticTextProvider provider, IFormatProvider formatProvider, CancellationToken cancellationToken)` method
   - Fetches localized labels for "Credit", "Balance", "Available" using `StaticTextKey.Credit`, `StaticTextKey.Balance`, `StaticTextKey.Available`
   - Formats each value using `.ToString("C", formatProvider)`

3. **`IAccountTexts` interface extended** with four new read-only properties:
   - `StatusDate` (IValueDisplayer)
   - `ValuesAtStatusDate` (IAccountValuesDisplayer)
   - `ValuesAtEndOfLastMonthFromStatusDate` (IAccountValuesDisplayer)
   - `ValuesAtEndOfLastYearFromStatusDate` (IAccountValuesDisplayer)

4. **`AccountTexts` class updated** to:
   - Accept all four new properties in constructor (after existing parameters)
   - Initialize properties in constructor body

5. **`AccountTextsBuilder.BuildAsync()` rewritten** to:
   - Create `StatusDate` using `GetStatusDateAsync()` helper from base class (format: "d", like "24/07/2026")
   - Build three `IAccountValuesDisplayer` instances in parallel using `Task.ContinueWith()`:
     - `ValuesAtStatusDate` from `AccountModel.ValuesAtStatusDate` with key `AccountValuesAtStatusDate`
     - `ValuesAtEndOfLastMonthFromStatusDate` from `AccountModel.ValuesAtEndOfLastMonthFromStatusDate` with key `AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate`
     - `ValuesAtEndOfLastYearFromStatusDate` from `AccountModel.ValuesAtEndOfLastYearFromStatusDate` with key `AccountValuesAtEndOfLastYearFromStatusDate`
   - Await all tasks with `Task.WhenAll()`
   - Return `new AccountTexts(model, statusDate!, valuesAtStatusDate!, …, formatProvider)`

6. **Three new `StaticTextKey` enum entries** added to [StaticTextKey.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs):
   - `AccountValuesAtStatusDate`
   - `AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate`
   - `AccountValuesAtEndOfLastYearFromStatusDate`

7. **Three new entries in `StaticTextProvider.GenerateStaticTexts()`** at [StaticTextProvider.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs):
   - `AccountValuesAtStatusDate` → "Kontoværdi pr. dags dato"
   - `AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate` → "Kontoværdi ved sidste måneds afslutning"
   - `AccountValuesAtEndOfLastYearFromStatusDate` → "Kontoværdi ved sidste års afslutning"

8. **Unit tests for `AccountValuesDisplayer.CreateAsync()`** at [OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountValuesDisplayerTests.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountValuesDisplayerTests.cs):
   - Test: parameter validation (null checks for provider, values, formatProvider)
   - Test: correct static text keys fetched for each label
   - Test: value formatting applied with currency format ("C")
   - Test: all properties assigned correctly to returned instance
   - Test: exception propagation from provider

9. **Comprehensive unit tests for `AccountTextsBuilder.BuildAsync()`** at [OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountTextsBuilderTests.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountTextsBuilderTests.cs):
   - Test: all four displayers created (StatusDate + three IAccountValuesDisplayer)
   - Test: parallel task construction verified (mock provider called concurrently)
   - Test: all properties assigned to returned IAccountTexts instance
   - Test: exception handling (provider throws, builder propagates)
   - Test: null model handling
   - Test: cancellation token passed through

10. **Three test cases added** to `GetStaticTextAsyncTests.GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText` at [OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/StaticText/GetStaticTextAsyncTests.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/StaticText/GetStaticTextAsyncTests.cs):
    - `[TestCase(StaticTextKey.AccountValuesAtStatusDate, "Kontoværdi pr. dags dato", 0)]`
    - `[TestCase(StaticTextKey.AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate, "Kontoværdi ved sidste måneds afslutning", 0)]`
    - `[TestCase(StaticTextKey.AccountValuesAtEndOfLastYearFromStatusDate, "Kontoværdi ved sidste års afslutning", 0)]`

### Phase 2: Budget Accounts (IBudgetAccountTexts, IBudgetAccountValuesDisplayer)

1. **New interface `IBudgetAccountValuesDisplayer`** exists with read-only properties:
   - `Header` (string)
   - `Budget` (IValueDisplayer)
   - `Posted` (IValueDisplayer)
   - `Available` (IValueDisplayer)

2. **New class `BudgetAccountValuesDisplayer`** (internal) with same structure as `AccountValuesDisplayer`:
   - Static `CreateAsync(StaticTextKey headerKey, BudgetInfoValuesModel values, IStaticTextProvider provider, IFormatProvider formatProvider, CancellationToken)` method
   - Fetches labels for "Budget", "Posted", "Available" using `StaticTextKey.Budget`, `StaticTextKey.Posted`, `StaticTextKey.Available`

3. **`IBudgetAccountTexts` interface extended** with five new read-only properties:
   - `StatusDate` (IValueDisplayer)
   - `ValuesForMonthOfStatusDate` (IBudgetAccountValuesDisplayer)
   - `ValuesForLastMonthOfStatusDate` (IBudgetAccountValuesDisplayer)
   - `ValuesForYearToDateOfStatusDate` (IBudgetAccountValuesDisplayer)
   - `ValuesForLastYearOfStatusDate` (IBudgetAccountValuesDisplayer)

4. **`BudgetAccountTexts` class updated** to accept and initialize all five new properties

5. **`BudgetAccountTextsBuilder.BuildAsync()` rewritten** to:
   - Create `StatusDate` via `GetStatusDateAsync()`
   - Build four `IBudgetAccountValuesDisplayer` instances in parallel:
     - `ValuesForMonthOfStatusDate` from `BudgetAccountModel.ValuesForMonthOfStatusDate` with key `BudgetAccountValuesForMonthOfStatusDate`
     - `ValuesForLastMonthOfStatusDate` from `BudgetAccountModel.ValuesForLastMonthOfStatusDate` with key `BudgetAccountValuesForLastMonthOfStatusDate`
     - `ValuesForYearToDateOfStatusDate` from `BudgetAccountModel.ValuesForYearToDateOfStatusDate` with key `BudgetAccountValuesForYearToDateOfStatusDate`
     - `ValuesForLastYearOfStatusDate` from `BudgetAccountModel.ValuesForLastYearOfStatusDate` with key `BudgetAccountValuesForLastYearOfStatusDate`
   - Await all tasks and return constructed instance

6. **Four new `StaticTextKey` enum entries** added:
   - `BudgetAccountValuesForMonthOfStatusDate`
   - `BudgetAccountValuesForLastMonthOfStatusDate`
   - `BudgetAccountValuesForYearToDateOfStatusDate`
   - `BudgetAccountValuesForLastYearOfStatusDate`

7. **Four new entries in `StaticTextProvider.GenerateStaticTexts()`**:
   - `BudgetAccountValuesForMonthOfStatusDate` → "Budgetoplysninger pr. dags dato"
   - `BudgetAccountValuesForLastMonthOfStatusDate` → "Budgetoplysninger ved sidste måneds afslutning"
   - `BudgetAccountValuesForYearToDateOfStatusDate` → "Budgetoplysninger for år til dato"
   - `BudgetAccountValuesForLastYearOfStatusDate` → "Budgetoplysninger ved sidste års afslutning"

8. **Comprehensive unit tests** for both `BudgetAccountValuesDisplayer.CreateAsync()` and `BudgetAccountTextsBuilder.BuildAsync()` mirroring Phase 1 structure

9. **Four test cases added** to `GetStaticTextAsyncTests`:
    - `[TestCase(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, "Budgetoplysninger pr. dags dato", 0)]`
    - `[TestCase(StaticTextKey.BudgetAccountValuesForLastMonthOfStatusDate, "Budgetoplysninger ved sidste måneds afslutning", 0)]`
    - `[TestCase(StaticTextKey.BudgetAccountValuesForYearToDateOfStatusDate, "Budgetoplysninger for år til dato", 0)]`
    - `[TestCase(StaticTextKey.BudgetAccountValuesForLastYearOfStatusDate, "Budgetoplysninger ved sidste års afslutning", 0)]`

### Phase 3: Contact Accounts (IContactAccountTexts, IContactAccountValuesDisplayer)

1. **New interface `IContactAccountValuesDisplayer`** with read-only properties:
   - `Header` (string)
   - `Balance` (IValueDisplayer)

2. **New class `ContactAccountValuesDisplayer`** (internal):
   - Static `CreateAsync(StaticTextKey headerKey, BalanceInfoValuesModel values, IStaticTextProvider provider, IFormatProvider formatProvider, CancellationToken)` method
   - Fetches label for "Balance" using `StaticTextKey.Balance`

3. **`IContactAccountTexts` interface extended** with four new read-only properties:
   - `StatusDate` (IValueDisplayer)
   - `ValuesAtStatusDate` (IContactAccountValuesDisplayer)
   - `ValuesAtEndOfLastMonthFromStatusDate` (IContactAccountValuesDisplayer)
   - `ValuesAtEndOfLastYearFromStatusDate` (IContactAccountValuesDisplayer)

4. **`ContactAccountTexts` class updated** to accept and initialize all four new properties

5. **`ContactAccountTextsBuilder.BuildAsync()` rewritten** to:
   - Create `StatusDate` via `GetStatusDateAsync()`
   - Build three `IContactAccountValuesDisplayer` instances in parallel:
     - `ValuesAtStatusDate` from `ContactAccountModel.ValuesAtStatusDate` with key `ContactAccountValuesAtStatusDate`
     - `ValuesAtEndOfLastMonthFromStatusDate` from `ContactAccountModel.ValuesAtEndOfLastMonthFromStatusDate` with key `ContactAccountValuesAtEndOfLastMonthFromStatusDate`
     - `ValuesAtEndOfLastYearFromStatusDate` from `ContactAccountModel.ValuesAtEndOfLastYearFromStatusDate` with key `ContactAccountValuesValuesAtEndOfLastYearFromStatusDate`

6. **Three new `StaticTextKey` enum entries** added:
   - `ContactAccountValuesAtStatusDate`
   - `ContactAccountValuesAtEndOfLastMonthFromStatusDate`
   - `ContactAccountValuesValuesAtEndOfLastYearFromStatusDate`

7. **Three new entries in `StaticTextProvider.GenerateStaticTexts()`**:
   - `ContactAccountValuesAtStatusDate` → "Saldooplysninger pr. dags dato"
   - `ContactAccountValuesAtEndOfLastMonthFromStatusDate` → "Saldooplysninger ved sidste måneds afslutning"
   - `ContactAccountValuesValuesAtEndOfLastYearFromStatusDate` → "Saldooplysninger ved sidste års afslutning"

8. **Comprehensive unit tests** for both `ContactAccountValuesDisplayer.CreateAsync()` and `ContactAccountTextsBuilder.BuildAsync()` mirroring earlier phases

9. **Three test cases added** to `GetStaticTextAsyncTests`:
    - `[TestCase(StaticTextKey.ContactAccountValuesAtStatusDate, "Saldooplysninger pr. dags dato", 0)]`
    - `[TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastMonthFromStatusDate, "Saldooplysninger ved sidste måneds afslutning", 0)]`
    - `[TestCase(StaticTextKey.ContactAccountValuesValuesAtEndOfLastYearFromStatusDate, "Saldooplysninger ved sidste års afslutning", 0)]`

### Cross-Phase Integration

10. **All unit tests pass**: `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"`

11. **Solution builds without errors or warnings**: `dotnet build OSDevGrp.OSIntranet.Applications.sln`

12. **All builders remain registered in DI**: [ServiceCollectionExtensions.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/ServiceCollectionExtensions.cs) already registers them; no new registration needed (they're already wired as transient)

13. **TODO.md updated with completion summary**: A new "✅ COMPLETED: Extend dynamic texts for accounts, budget accounts, and contact accounts" section is added to [../../TODO.md](../../TODO.md) with:
    - Implementation Summary (noting which commit(s) completed this work)
    - Files Modified (list of all interface/implementation/provider files changed)
    - Files Created (all new displayer classes, test files)
    - Verification (build passes, all unit tests pass, DI registration verified)
    - Pattern matching the existing "✅ COMPLETED: Extend IAccountingGateway..." section

## Scope

### In scope
- Three new displayer interfaces + implementations (`IAccountValuesDisplayer`, `IBudgetAccountValuesDisplayer`, `IContactAccountValuesDisplayer`)
- Extensions to three text interfaces (`IAccountTexts`, `IBudgetAccountTexts`, `IContactAccountTexts`)
- Updates to three text classes to store and expose new displayers
- Rewrite of three builder classes to construct displayers in parallel
- 10 new `StaticTextKey` enum entries
- 10 corresponding Danish label strings in `StaticTextProvider`
- Comprehensive unit tests for all new displayers and builders
- 9 new test cases in `GetStaticTextAsyncTests`

### Out of scope
- Domain service layer changes — builders are wired but not called by any service yet (prep for future use)
- React frontend changes — UI integration deferred
- API contract changes — models already exist with required properties
- Posting journal extensions (separate TODO item; not part of account/budget/contact scope)

## Risks

### High: Constructor Signature Changes
**Risk:** Adding four properties to `AccountTexts`, five to `BudgetAccountTexts`, and four to `ContactAccountTexts` requires updating constructor signatures. If there are callsites outside the builders, they'll break.
**Mitigation:** Search codebase for any direct instantiation of these classes (grep for `new AccountTexts(`, `new BudgetAccountTexts(`, `new ContactAccountTexts(`). If found outside builders, update them. Current expectation: only builders instantiate these classes.

### Medium: Parallel Task Orchestration
**Risk:** Converting to parallel `Task.ContinueWith()` + `Task.WhenAll()` pattern (especially in the simple builders that currently do nothing) could introduce race conditions or task cancellation issues if not careful.
**Mitigation:** Use exact pattern from `AccountingTextsBuilder` as template. Test cancellation token propagation. Verify all tasks capture variables correctly (closure behavior).

### Medium: Model Property Dependencies
**Risk:** If `AccountModel.ValuesAtStatusDate` or similar properties are null, the displayers will attempt to format null decimals, producing incorrect output or exceptions.
**Mitigation:** `ValueDisplayer` handles null values gracefully (property is `string?`). Tests should verify behavior with zero and negative values. Document assumption: API always returns initialized `CreditInfoValuesModel` / `BudgetInfoValuesModel` / `BalanceInfoValuesModel` objects (even if values are 0).

### Low: StaticTextKey Naming
**Risk:** Typo in enum entry name or mismatch between enum and provider string key.
**Mitigation:** Carefully transcribe exact names from TODO. Use `typeof(StaticTextKey).GetFields()` reflection test to verify all keys defined in provider (or add runtime validation test).

### Low: Danish Text Accuracy
**Risk:** Typos in Danish translations provided by user.
**Mitigation:** User confirms translations in TODO are correct before implementation. If needed, translations can be corrected post-implementation in a follow-up.
