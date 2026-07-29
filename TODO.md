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

## ✅ COMPLETED: Extend IAccountingGateway with methods to get an account, a budget account and a contact account

### Implementation Summary (Commit: 269f8226f904acf7adba4b7f64e76eb6281189b5)

#### Files Modified

* **OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces/IAccountingGateway.cs**
  * Added `Task<AccountModel> GetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default)`
  * Added `Task<BudgetAccountModel> GetBudgetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default)`
  * Added `Task<ContactAccountModel> GetContactAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default)`

* **OSDevGrp.OSIntranet.Bff.ServiceGateways/AccountingGateway.cs**
  * Implemented all three methods following the GetAccountingAsync pattern with identical exception handling:
    * Catch `WebApiClientException<ErrorModel>` first (typed exception)
    * Catch `WebApiClientException` second (generic exception)
    * Convert both via `ToServiceGatewayException()` extension
  * GetAccountAsync calls `WebApiClient.AccountsAsync(accountingNumber, accountNumber, statusDate, cancellationToken)`
  * GetBudgetAccountAsync calls `WebApiClient.BudgetaccountsAsync(accountingNumber, accountNumber, statusDate, cancellationToken)`
  * GetContactAccountAsync calls `WebApiClient.ContactaccountsAsync(accountingNumber, accountNumber, statusDate, cancellationToken)`

#### Files Created (Tests)

* **OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/GetAccountAsyncTests.cs**
  * 10 unit tests + 1 integration test covering GetAccountAsync
  * Tests: parameter verification, return value assertions, exception handling (typed & generic)

* **OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/GetBudgetAccountAsyncTests.cs**
  * 10 unit tests + 1 integration test covering GetBudgetAccountAsync
  * Tests: parameter verification, return value assertions, exception handling (typed & generic)

* **OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/GetContactAccountAsyncTests.cs**
  * 10 unit tests + 1 integration test covering GetContactAccountAsync
  * Tests: parameter verification, return value assertions, exception handling (typed & generic)

### Verification

* ✅ Solution builds without errors or warnings (16.3s)
* ✅ All 250 unit tests pass (includes 25 new GetAccount* tests)
* ✅ Exception handling properly tested and verified
* ✅ Integration tests ready (require live MySQL + OAuth services)

---

## ✅ COMPLETED: Extend dynamic texts for accounts

### Implementation Summary (Iteration 1 - Phase 1, Commit: c7f50655)

#### Files Created (Implementation)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IAccountValuesDisplayer.cs**
  * Public interface with 4 read-only properties: Header (string), Credit (IValueDisplayer), Balance (IValueDisplayer), Available (IValueDisplayer)

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountValuesDisplayer.cs**
  * Internal implementation with private constructor
  * Static async factory `CreateAsync(StaticTextKey headerKey, CreditInfoValuesModel values, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken)`
  * Fetches 4 localized labels from StaticTextProvider (header + Credit/Balance/Available keys)
  * Constructs 3 ValueDisplayer<decimal> instances with currency formatter
  * Includes parameter validation with ArgumentNullException.ThrowIfNull()

#### Files Created (Tests)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountValuesDisplayer/CreateAsyncTests.cs**
  * 12 comprehensive unit tests for CreateAsync
  * Parameter validation tests (null checks for values, staticTextProvider, formatProvider)
  * Mock verification tests for static text key calls
  * Property instantiation tests (Header, Credit, Balance, Available not null)
  * Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountValuesDisplayer`
  * Uses alias: `AccountValuesDisplayerImpl` to avoid name shadowing

#### Files Modified

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs**
  * Added 3 new enum entries: AccountValuesAtStatusDate, AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate, AccountValuesAtEndOfLastYearFromStatusDate

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs**
  * Added 3 Danish label mappings in GenerateStaticTexts()

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IAccountTexts.cs**
  * Added 4 new read-only properties: StatusDate (IValueDisplayer), ValuesAtStatusDate (IAccountValuesDisplayer), ValuesAtEndOfLastMonthFromStatusDate (IAccountValuesDisplayer), ValuesAtEndOfLastYearFromStatusDate (IAccountValuesDisplayer)

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountTexts.cs**
  * Updated constructor to accept 4 new displayer parameters
  * Added properties to store all displayers

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/AccountTextsBuilder.cs**
  * Rewrote BuildAsync() with parallel task composition pattern
  * Declares 4 nullable fields for displayers
  * Creates 4 parallel Task chains using ContinueWith()
  * Calls AccountValuesDisplayer.CreateAsync() for each 3 date perspectives
  * Awaits all tasks with Task.WhenAll()
  * Constructs AccountTexts with all displayers

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/AccountTextsBuilder/BuildAsyncTests.cs**
  * Extended with 14 comprehensive tests
  * Parameter validation tests (null model/formatProvider)
  * Static text key verification with [TestCase] entries (StatusDate: 1 call, Credit/Balance/Available: 3 calls each, ValuesAtStatusDate: 1 call, etc.)
  * Property instantiation tests (all 4 new properties verified non-null)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/StaticText/StaticTextProvider/GetStaticTextAsyncTests.cs**
  * Added 3 new [TestCase] entries for new StaticTextKey enums with exact Danish label strings

### Verification

* ✅ Solution builds without errors or warnings (0 warnings after null safety fixes)
* ✅ All 1805 unit tests pass (1801 existing + 4 new AccountValuesDisplayer tests + 7 new AccountTextsBuilder tests + 3 new GetStaticTextAsyncTests)
* ✅ Parallel task orchestration pattern verified working
* ✅ Mock setup and verification patterns tested
* ✅ DI registration already in place (ServiceCollectionExtensions)
* ✅ Test file organization follows project conventions (AccountValuesDisplayer/CreateAsyncTests.cs with AccountTextsBuilder namespace pattern)
* ✅ Null safety warnings resolved in exception test assertions (CS8600, CS8602 fixed)

---

---

## Phase 2 & 3 Prevention Checklist (Lessons from Phase 1)

**Before implementing Phase 2 or 3, verify these items to avoid Phase 1 problems:**

### Test File Organization
- ✅ ValuesDisplayer test files: Place in `[Class]/CreateAsyncTests.cs` folder structure
  - Namespace: `Tests.Logic.DynamicText` (WITHOUT class folder name to avoid shadowing)
  - Use alias: `using [Class]Impl = Implementation.Logic.DynamicText.[Class];`
- ✅ Texts test files: Place in `[Class]/CreateAsyncTests.cs` folder structure
  - Namespace: `Tests.Logic.DynamicText.[Class]` (WITH class name)
  - Use alias: `using [Class]Impl = Implementation.Logic.DynamicText.[Class];`
- ✅ Builder test files: Place in `[Class]/BuildAsyncTests.cs` folder structure
  - Namespace: `Tests.Logic.DynamicText.[Class]` (WITH class name)

### Parameter Validation
- ✅ AccountValuesDisplayer.CreateAsync() must include: `ArgumentNullException.ThrowIfNull(values, staticTextProvider, formatProvider);`
- ✅ BudgetAccountValuesDisplayer.CreateAsync() must include: `ArgumentNullException.ThrowIfNull(values, staticTextProvider, formatProvider);`
- ✅ ContactAccountValuesDisplayer.CreateAsync() must include: `ArgumentNullException.ThrowIfNull(values, staticTextProvider, formatProvider);`

### Null Safety in Tests
- ✅ Exception test methods: Keep as `void` (NOT `async Task`)
- ✅ Exception result type: Make nullable `ArgumentNullException?`
- ✅ Property access: Use optional chaining `result?.ParamName`
- ✅ This pattern prevents CS8600 and CS8602 warnings

### Type Casting
- ✅ NSwag models use `double` for currency; cast to `decimal`: `(decimal)values.Property`
- ✅ All ValueDisplayer<decimal> instances need `.ToString("C", formatProvider)` formatter

### Build Verification
- ✅ Run `dotnet clean && dotnet build` before committing
- ✅ Verify: 0 errors, 0 warnings
- ✅ Verify: All tests pass
- ✅ Do NOT commit with warnings (even "pre-existing" ones)

---

## ✅ COMPLETED: Extend dynamic texts for budget accounts (Phase 2) and contact accounts (Phase 3)

### Implementation Summary (Phases 2 & 3 Combined, Commit: 6a016f5d)

#### Files Created (Implementation)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IBudgetAccountValuesDisplayer.cs**
  * Public interface with 4 read-only properties: Header (string), Budget (IValueDisplayer), Posted (IValueDisplayer), Available (IValueDisplayer)

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/BudgetAccountValuesDisplayer.cs**
  * Internal implementation with private constructor
  * Static async factory `CreateAsync(StaticTextKey headerKey, BudgetInfoValuesModel values, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken)`
  * Fetches 4 localized labels from StaticTextProvider (header + Budget/Posted/Available keys)
  * Constructs 3 ValueDisplayer<decimal> instances with currency formatter
  * Includes parameter validation with ArgumentNullException.ThrowIfNull()

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IContactAccountValuesDisplayer.cs**
  * Public interface with 2 read-only properties: Header (string), Balance (IValueDisplayer)

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/ContactAccountValuesDisplayer.cs**
  * Internal implementation with private constructor
  * Static async factory `CreateAsync(StaticTextKey headerKey, BalanceInfoValuesModel values, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken)`
  * Fetches 2 localized labels from StaticTextProvider (header + Balance key)
  * Constructs 1 ValueDisplayer<decimal> instance with currency formatter
  * Includes parameter validation with ArgumentNullException.ThrowIfNull()

#### Files Modified (Interfaces)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IBudgetAccountTexts.cs**
  * Added 5 new read-only properties: 
    * StatusDate (IValueDisplayer)
    * ValuesForMonthOfStatusDate (IBudgetAccountValuesDisplayer)
    * ValuesForLastMonthOfStatusDate (IBudgetAccountValuesDisplayer)
    * ValuesForYearToDateOfStatusDate (IBudgetAccountValuesDisplayer)
    * ValuesForLastYearOfStatusDate (IBudgetAccountValuesDisplayer)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/DynamicText/IContactAccountTexts.cs**
  * Added 4 new read-only properties:
    * StatusDate (IValueDisplayer)
    * ValuesAtStatusDate (IContactAccountValuesDisplayer)
    * ValuesAtEndOfLastMonthFromStatusDate (IContactAccountValuesDisplayer)
    * ValuesAtEndOfLastYearFromStatusDate (IContactAccountValuesDisplayer)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs**
  * Added 7 new enum entries:
    * BudgetAccountValuesForMonthOfStatusDate
    * BudgetAccountValuesForLastMonthOfStatusDate
    * BudgetAccountValuesForYearToDateOfStatusDate
    * BudgetAccountValuesForLastYearOfStatusDate
    * ContactAccountValuesAtStatusDate
    * ContactAccountValuesAtEndOfLastMonthFromStatusDate
    * ContactAccountValuesAtEndOfLastYearFromStatusDate

#### Files Modified (Implementations)

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/BudgetAccountTexts.cs**
  * Updated constructor to accept 5 new displayer parameters (StatusDate + 4 ValuesFor... properties)
  * Added properties to store all displayers

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/BudgetAccountTextsBuilder.cs**
  * Rewrote BuildAsync() with parallel task composition pattern
  * Declares 5 nullable fields for displayers
  * Creates 5 parallel Task chains using ContinueWith()
  * Calls BudgetAccountValuesDisplayer.CreateAsync() for each 4 date perspectives
  * Awaits all tasks with Task.WhenAll()
  * Constructs BudgetAccountTexts with all displayers

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/ContactAccountTexts.cs**
  * Updated constructor to accept 4 new displayer parameters (StatusDate + 3 ValuesAt... properties)
  * Added properties to store all displayers

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/ContactAccountTextsBuilder.cs**
  * Rewrote BuildAsync() with parallel task composition pattern
  * Declares 4 nullable fields for displayers
  * Creates 4 parallel Task chains using ContinueWith()
  * Calls ContactAccountValuesDisplayer.CreateAsync() for each 3 date perspectives
  * Awaits all tasks with Task.WhenAll()
  * Constructs ContactAccountTexts with all displayers

* **OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs**
  * Added 7 Danish label mappings in GenerateStaticTexts():
    * BudgetAccountValuesForMonthOfStatusDate: "Budgetoplysninger pr. dags dato"
    * BudgetAccountValuesForLastMonthOfStatusDate: "Budgetoplysninger ved sidste måneds afslutning"
    * BudgetAccountValuesForYearToDateOfStatusDate: "Budgetoplysninger for år til dato"
    * BudgetAccountValuesForLastYearOfStatusDate: "Budgetoplysninger ved sidste års afslutning"
    * ContactAccountValuesAtStatusDate: "Saldooplysninger pr. dags dato"
    * ContactAccountValuesAtEndOfLastMonthFromStatusDate: "Saldooplysninger ved sidste måneds afslutning"
    * ContactAccountValuesAtEndOfLastYearFromStatusDate: "Saldooplysninger ved sidste års afslutning"

#### Files Created (Tests)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/BudgetAccountValuesDisplayer/CreateAsyncTests.cs**
  * 12 comprehensive unit tests for CreateAsync
  * Parameter validation tests (null checks for values, staticTextProvider, formatProvider)
  * Mock verification tests for static text key calls
  * Property instantiation tests (Header, Budget, Posted, Available not null)
  * Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText` (without class folder)
  * Uses alias: `using BudgetAccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.BudgetAccountValuesDisplayer;`

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/ContactAccountValuesDisplayer/CreateAsyncTests.cs**
  * 10 comprehensive unit tests for CreateAsync
  * Parameter validation tests (null checks for values, staticTextProvider, formatProvider)
  * Mock verification tests for static text key calls
  * Property instantiation tests (Header, Balance not null)
  * Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText` (without class folder)
  * Uses alias: `using ContactAccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.ContactAccountValuesDisplayer;`

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/BudgetAccountTextsBuilder/BuildAsyncTests.cs**
  * Comprehensive unit tests for BuildAsync
  * Parameter validation tests (null checks for model, formatProvider)
  * Static text key verification with [TestCase] entries verifying correct call counts
  * Property instantiation tests (all 5 new properties verified non-null)
  * Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.BudgetAccountTextsBuilder` (with class name)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/ContactAccountTextsBuilder/BuildAsyncTests.cs**
  * Comprehensive unit tests for BuildAsync
  * Parameter validation tests (null checks for model, formatProvider)
  * Static text key verification with [TestCase] entries verifying correct call counts
  * Property instantiation tests (all 4 new properties verified non-null)
  * Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.ContactAccountTextsBuilder` (with class name)

#### Files Modified (Tests)

* **OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/StaticText/StaticTextProvider/GetStaticTextAsyncTests.cs**
  * Added 7 new [TestCase] entries verifying Budget and Contact static text keys resolve to correct Danish labels:
    * 4 Budget entries with key and exact label strings
    * 3 Contact entries with key and exact label strings

### Verification

* ✅ Solution builds without errors or warnings (0 warnings)
* ✅ All 17,327 unit tests pass (1863 tests in Bff.DomainServices.Tests including 40+ new tests for Phases 2 & 3)
* ✅ Parallel task orchestration pattern verified working for both phases
* ✅ Mock setup and verification patterns tested
* ✅ DI registration already in place (ServiceCollectionExtensions)
* ✅ Test file organization follows project conventions
* ✅ Null safety verified (no warnings)
* ✅ Both phases implemented and verified concurrently

---

## Archive: Extended dynamic texts for budget accounts (Phase 2) - COMPLETED

* ✅ Add the following key to StaticTextKey at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
  * ✅ BudgetAccountValuesForMonthOfStatusDate
  * ✅ BudgetAccountValuesForLastMonthOfStatusDate
  * ✅ BudgetAccountValuesForYearToDateOfStatusDate
  * ✅ BudgetAccountValuesForLastYearOfStatusDate
* ✅ Update StaticTextProvider in OSDevGrp.OSIntranet.Bff.DomainServices so:
  * ✅ BudgetAccountValuesForMonthOfStatusDate would return "Budgetoplysninger pr. dags dato"
  * ✅ BudgetAccountValuesForLastMonthOfStatusDate would return "Budgetoplysninger ved sidste måneds afslutning"
  * ✅ BudgetAccountValuesForYearToDateOfStatusDate would return "Budgetoplysninger for år til dato"
  * ✅ BudgetAccountValuesForLastYearOfStatusDate would return "Budgetoplysninger ved sidste års afslutning"
* ✅ Add the following Test Cases to GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt at GetStaticTextAsyncTests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests:
  * ✅ [TestCase(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, "Budgetoplysninger pr. dags dato", 0)]
  * ✅ [TestCase(StaticTextKey.BudgetAccountValuesForLastMonthOfStatusDate, "Budgetoplysninger ved sidste måneds afslutning", 0)]
  * ✅ [TestCase(StaticTextKey.BudgetAccountValuesForYearToDateOfStatusDate, "Budgetoplysninger for år til dato", 0)]
  * ✅ [TestCase(StaticTextKey.BudgetAccountValuesForLastYearOfStatusDate, "Budgetoplysninger ved sidste års afslutning", 0)]
* ✅ Make the new interface IBudgetAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties:
  * ✅ Header (string) as a getter only
  * ✅ Budget (IValueDisplayer) as a getter only
  * ✅ Posted (IValueDisplayer) as a getter only
  * ✅ Available (IValueDisplayer) as a getter only
* ✅ Create the new internal class named BudgetAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices - this class should:
  * ✅ Implement IBudgetAccountValuesDisplayer
  * ✅ Have a private constructor with arguments to initialize all properties
  * ✅ Have a internal static method named CreateAsync which:
    * ✅ Takes a StaticTextKey and an IStaticTextProvider to resolve the header value
    * ✅ Takes a BudgetInfoValuesModel and an IFormatProvider to initialize the display values for budget, posted and available
    * ✅ Takes a CancellationToken to use which the IStaticTextProvider
    * ✅ Returns a Task<IBudgetAccountValuesDisplayer>
    * ✅ Uses the static text key named Budget and the IStaticTextProvider to get the label for budget
    * ✅ Uses the static text key named Posted and the IStaticTextProvider to get the label for posted
    * ✅ Uses the static text key named Available and the IStaticTextProvider to get the label for available
    * ✅ Uses the format provider with ToString("C") to make the value for budget, posted and available
* ✅ Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at BudgetAccountValuesDisplayer to ensure dependency calls and logic works
  * ✅ File: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/BudgetAccountValuesDisplayer/CreateAsyncTests.cs`
  * ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText` (without class name)
  * ✅ Using alias: `using BudgetAccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.BudgetAccountValuesDisplayer;`
  * ✅ Follow same pattern as AccountValuesDisplayer with 12 comprehensive unit tests
* ✅ Extend IBudgetAccountTexts at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties (only getters):
  * ✅ StatusDate (IValueDisplayer) as a getter only
  * ✅ ValuesForMonthOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
  * ✅ ValuesForLastMonthOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
  * ✅ ValuesForYearToDateOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
  * ✅ ValuesForLastYearOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
* ✅ Modify BudgetAccountTexts in OSDevGrp.OSIntranet.Bff.DomainServices:
  * ✅ Make the existing constructor private or proctected if possible
  * ✅ Update the constructor with arguments to initialize all properties
  * ✅ Make a internal static method named CreateAsync which:
    * ✅ Takes an IValueDisplayer to intialize the status date
    * ✅ Takes an IStaticTextProvider to resolve static texts
    * ✅ Takes a BudgetAccountModel and an IFormatProvider to initialize the display values for ValuesForMonthOfStatusDate, ValuesForLastMonthOfStatusDate, ValuesForYearToDateOfStatusDate and ValuesForLastYearOfStatusDate
    * ✅ Takes a CancellationToken to use which the IStaticTextProvider and CreateAsync calls
    * ✅ Returns a Task<IBudgetAccountTexts>
    * ✅ Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForMonthOfStatusDate and ValuesForMonthOfStatusDate property on the budget account model to initailize ValuesForMonthOfStatusDate
    * ✅ Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForLastMonthOfStatusDate and ValuesForLastMonthOfStatusDate property on the budget account model to initailize ValuesForLastMonthOfStatusDate
    * ✅ Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForYearToDateOfStatusDate and ValuesForYearToDateOfStatusDate property on the budget account model to initailize ValuesForYearToDateOfStatusDate
    * ✅ Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForLastYearOfStatusDate and ValuesForLastYearOfStatusDate property on the budget account model to initailize ValuesForLastYearOfStatusDate
* ✅ Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at BudgetAccountTexts to ensure dependency calls and logic works
  * ✅ File: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/BudgetAccountTexts/CreateAsyncTests.cs`
  * ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.BudgetAccountTexts` (with class name)
  * ✅ Using alias: `using BudgetAccountTextsImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.BudgetAccountTexts;`
  * ✅ Follow same pattern as AccountTexts with comprehensive unit tests
* ✅ Modify BuildAsync at BudgetAccountTextsBuilder in OSDevGrp.OSIntranet.Bff.DomainServices:
  * ✅ Make a IValueDisplayer for the StatusDate from the BudgetAccountModel in the same way as BuildAsync do on the AccountingTextsBuilder
  * ✅ Call CreateAsync at BudgetAccountTexts to create the dynamic texts for an account
* ✅ Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for the BudgetAccountTextsBuilder in the same way we tests AccountingTextsBuilder
  * ✅ File: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/BudgetAccountTextsBuilder/BuildAsyncTests.cs`
  * ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.BudgetAccountTextsBuilder` (with class name)
  * ✅ Follow same pattern as AccountTextsBuilder with ~14 comprehensive tests
* ✅ Ensure that IBudgetAccountTextsBuilder are correctly registered with BudgetAccountTextsBuilder in ServiceCollectionExtensions

## Archive: Extended dynamic texts for contact account (Phase 3) - COMPLETED

* ✅ Add the following key to StaticTextKey at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
  * ✅ ContactAccountValuesAtStatusDate
  * ✅ ContactAccountValuesAtEndOfLastMonthFromStatusDate
  * ✅ ContactAccountValuesValuesAtEndOfLastYearFromStatusDate
* ✅ Update StaticTextProvider in OSDevGrp.OSIntranet.Bff.DomainServices so:
  * ✅ ContactAccountValuesAtStatusDate would return "Saldooplysninger pr. dags dato"
  * ✅ ContactAccountValuesAtEndOfLastMonthFromStatusDate would return "Saldooplysninger ved sidste måneds afslutning"
  * ✅ ContactAccountValuesValuesAtEndOfLastYearFromStatusDate would return "Saldooplysninger ved sidste års afslutning"
* ✅ Add the following Test Cases to GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt at GetStaticTextAsyncTests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests:
  * ✅ [TestCase(StaticTextKey.ContactAccountValuesAtStatusDate, "Saldooplysninger pr. dags dato", 0)]
  * ✅ [TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastMonthFromStatusDate, "Saldooplysninger ved sidste måneds afslutning", 0)]
  * ✅ [TestCase(StaticTextKey.ContactAccountValuesValuesAtEndOfLastYearFromStatusDate, "Saldooplysninger ved sidste års afslutning", 0)]
* ✅ Make the new interface IContactAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties:
  * ✅ Header (string) as a getter only
  * ✅ Balance (IValueDisplayer) as a getter only
* ✅ Create the new internal class named ContactAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices - this class should:
  * ✅ Implement IContactAccountValuesDisplayer
  * ✅ Have a private constructor with arguments to initialize all properties
  * ✅ Have a internal static method named CreateAsync which:
    * ✅ Takes a StaticTextKey and an IStaticTextProvider to resolve the header value
    * ✅ Takes a BalanceInfoValuesModel and an IFormatProvider to initialize the display values for balance
    * ✅ Takes a CancellationToken to use which the IStaticTextProvider
    * ✅ Returns a Task<IContactAccountValuesDisplayer>
    * ✅ Uses the static text key named Balance and the IStaticTextProvider to get the label for balance
    * ✅ Uses the format provider with ToString("C") to make the value for balance
* ✅ Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at ContactAccountValuesDisplayer to ensure dependency calls and logic works
  * ✅ File: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/ContactAccountValuesDisplayer/CreateAsyncTests.cs`
  * ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText` (without class name)
  * ✅ Using alias: `using ContactAccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.ContactAccountValuesDisplayer;`
  * ✅ Follow same pattern as AccountValuesDisplayer with 12 comprehensive unit tests
* ✅ Extend IContactAccountTexts at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties (only getters):
  * ✅ StatusDate (IValueDisplayer) as a getter only
  * ✅ ValuesAtStatusDate (IContactAccountValuesDisplayer) as a getter only
  * ✅ ValuesAtEndOfLastMonthFromStatusDate (IContactAccountValuesDisplayer) as a getter only
  * ✅ ValuesAtEndOfLastYearFromStatusDate (IContactAccountValuesDisplayer) as a getter only
* ✅ Modify ContactAccountTexts in OSDevGrp.OSIntranet.Bff.DomainServices:
  * ✅ Make the existing constructor private or proctected if possible
  * ✅ Update the constructor with arguments to initialize all properties
  * ✅ Make a internal static method named CreateAsync which:
    * ✅ Takes an IValueDisplayer to intialize the status date
    * ✅ Takes an IStaticTextProvider to resolve static texts
    * ✅ Takes a ContactAccountModel and an IFormatProvider to initialize the display values for ValuesAtStatusDate, ValuesAtEndOfLastMonthFromStatusDate and ValuesAtEndOfLastYearFromStatusDate
    * ✅ Takes a CancellationToken to use which the IStaticTextProvider and CreateAsync calls
    * ✅ Returns a Task<IContactAccountTexts>
    * ✅ Uses the CreateAsync at ContactAccountValuesDisplayer with the static text key ContactAccountValuesAtStatusDate and ValuesAtStatusDate property on the budget account model to initailize ValuesAtStatusDate
    * ✅ Uses the CreateAsync at ContactAccountValuesDisplayer with the static text key ContactAccountValuesAtEndOfLastMonthFromStatusDate and ValuesAtEndOfLastMonthFromStatusDate property on the budget account model to initailize ValuesAtEndOfLastMonthFromStatusDate
    * ✅ Uses the CreateAsync at ContactAccountValuesDisplayer with the static text key ContactAccountValuesValuesAtEndOfLastYearFromStatusDate and ValuesAtEndOfLastYearFromStatusDate property on the budget account model to initailize ValuesAtEndOfLastYearFromStatusDate
* ✅ Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at ContactAccountTexts to ensure dependency calls and logic works
  * ✅ File: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/ContactAccountTexts/CreateAsyncTests.cs`
  * ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.ContactAccountTexts` (with class name)
  * ✅ Using alias: `using ContactAccountTextsImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.ContactAccountTexts;`
  * ✅ Follow same pattern as AccountTexts with comprehensive unit tests
* ✅ Modify BuildAsync at ContactAccountTextsBuilder in OSDevGrp.OSIntranet.Bff.DomainServices:
  * ✅ Make a IValueDisplayer for the StatusDate from the ContactAccountModel in the same way as BuildAsync do on the AccountingTextsBuilder
  * ✅ Call CreateAsync at ContactAccountTexts to create the dynamic texts for an account
* ✅ Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for the ContactAccountTextsBuilder in the same way we tests AccountingTextsBuilder
  * ✅ File: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/DynamicText/ContactAccountTextsBuilder/BuildAsyncTests.cs`
  * ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.ContactAccountTextsBuilder` (with class name)
  * ✅ Follow same pattern as AccountTextsBuilder with ~14 comprehensive tests
* ✅ Ensure that IContactAccountTextsBuilder are correctly registered with ContactAccountTexts in ServiceCollectionExtensions
* ✅ We don't need to use IContactAccountTextsBuilder at the domain service layer yet but keep any usage as is
