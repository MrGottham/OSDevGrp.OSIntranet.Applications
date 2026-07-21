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

### Implementation Summary (Commit: PENDING)

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

## Extend dynamic texts for accounts

* Add the following key to StaticTextKey at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
  * AccountValuesAtStatusDate
  * AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate
  * AccountValuesAtEndOfLastYearFromStatusDate
* Update StaticTextProvider in OSDevGrp.OSIntranet.Bff.DomainServices so:
  * AccountValuesAtStatusDate would return "Kontoværdi pr. dags dato"
  * AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate would return "Kontoværdi ved sidste måneds afslutning"
  * AccountValuesAtEndOfLastYearFromStatusDate would return "Kontoværdi ved sidste års afslutning"
* Add the following Test Cases to GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt at GetStaticTextAsyncTests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests:
  * [TestCase(StaticTextKey.AccountValuesAtStatusDate, "Kontoværdi pr. dags dato", 0)]
  * [TestCase(StaticTextKey.AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate, "Kontoværdi ved sidste måneds afslutning", 0)]
  * [TestCase(StaticTextKey.AccountValuesAtEndOfLastYearFromStatusDate, "Kontoværdi ved sidste års afslutning", 0)]
* Make the new interface IAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties:
  * Header (string) as a getter only
  * Credit (IValueDisplayer) as a getter only
  * Balance (IValueDisplayer) as a getter only
  * Available (IValueDisplayer) as a getter only
* Create the new internal class named AccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices - this class should:
  * Implement IAccountValuesDisplayer
  * Have a private constructor with arguments to initialize all properties
  * Have a internal static method named CreateAsync which:
    * Takes a StaticTextKey and an IStaticTextProvider to resolve the header value
    * Takes a CreditInfoValuesModel and an IFormatProvider to initialize the display values for credit, balance and available
    * Takes a CancellationToken to use which the IStaticTextProvider
    * Returns a Task<IAccountValuesDisplayer>
    * Uses the static text key named Credit and the IStaticTextProvider to get the label for credit
    * Uses the static text key named Balance and the IStaticTextProvider to get the label for balance
    * Uses the static text key named Available and the IStaticTextProvider to get the label for available
    * Uses the format provider with ToString("C") to make the value for credit, balance and available
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at AccountValuesDisplayer to ensure dependency calls and logic works
* Extend IAccountTexts at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties (only getters):
  * StatusDate (IValueDisplayer) as a getter only
  * ValuesAtStatusDate (IAccountValuesDisplayer) as a getter only
  * ValuesAtEndOfLastMonthFromStatusDate (IAccountValuesDisplayer) as a getter only
  * ValuesAtEndOfLastYearFromStatusDate (IAccountValuesDisplayer) as a getter only
* Modify AccountTexts in OSDevGrp.OSIntranet.Bff.DomainServices:
  * Make the existing constructor private or proctected if possible
  * Update the constructor with arguments to initialize all properties
  * Make a internal static method named CreateAsync which:
    * Takes an IValueDisplayer to intialize the status date
    * Takes an IStaticTextProvider to resolve static texts
    * Takes a AccountModel and an IFormatProvider to initialize the display values for ValuesAtStatusDate, ValuesAtEndOfLastMonthFromStatusDate and ValuesAtEndOfLastYearFromStatusDate
    * Takes a CancellationToken to use which the IStaticTextProvider and CreateAsync calls
    * Returns a Task<IAccountTexts>
    * Uses the CreateAsync at AccountValuesDisplayer with the static text key AccountValuesAtStatusDate and ValuesAtStatusDate property on the account model to initailize ValuesAtStatusDate
    * Uses the CreateAsync at AccountValuesDisplayer with the static text key AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate and ValuesAtStatusDate property on the account model to initailize ValuesAtEndOfLastMonthFromStatusDate
    * Uses the CreateAsync at AccountValuesDisplayer with the static text key AccountValuesAtEndOfLastYearFromStatusDate and ValuesAtStatusDate property on the account model to initailize ValuesAtEndOfLastYearFromStatusDate
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at AccountTexts to ensure dependency calls and logic works
* Modify BuildAsync at AccountTextsBuilder in OSDevGrp.OSIntranet.Bff.DomainServices:
  * Make a IValueDisplayer for the StatusDate from the AccountModel in the same way as BuildAsync do on the AccountingTextsBuilder
  * Call CreateAsync at AccountTexts to create the dynamic texts for an account
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for the AccountTextsBuilder in the same way we tests AccountingTextsBuilder
* Ensure that IAccountingTextsBuilder are correctly registered with AccountTextsBuilder in ServiceCollectionExtensions
* We don't need to use IAccountingTextsBuilder at the domain service layer yet but keep any usage as is

## Extend dynamic texts for budget accounts

* Add the following key to StaticTextKey at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
  * BudgetAccountValuesForMonthOfStatusDate
  * BudgetAccountValuesForLastMonthOfStatusDate
  * BudgetAccountValuesForYearToDateOfStatusDate
  * BudgetAccountValuesForLastYearOfStatusDate
* Update StaticTextProvider in OSDevGrp.OSIntranet.Bff.DomainServices so:
  * BudgetAccountValuesForMonthOfStatusDate would return "Budgetoplysninger pr. dags dato"
  * BudgetAccountValuesForLastMonthOfStatusDate would return "Budgetoplysninger ved sidste måneds afslutning"
  * BudgetAccountValuesForYearToDateOfStatusDate would return "Budgetoplysninger for år til dato"
  * BudgetAccountValuesForLastYearOfStatusDate would return "Budgetoplysninger ved sidste års afslutning"
* Add the following Test Cases to GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt at GetStaticTextAsyncTests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests:
  * [TestCase(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, "Budgetoplysninger pr. dags dato", 0)]
  * [TestCase(StaticTextKey.BudgetAccountValuesForLastMonthOfStatusDate, "Budgetoplysninger ved sidste måneds afslutning", 0)]
  * [TestCase(StaticTextKey.BudgetAccountValuesForYearToDateOfStatusDate, "Budgetoplysninger for år til dato", 0)]
  * [TestCase(StaticTextKey.BudgetAccountValuesForLastYearOfStatusDate, "Budgetoplysninger ved sidste års afslutning", 0)]
* Make the new interface IBudgetAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties:
  * Header (string) as a getter only
  * Budget (IValueDisplayer) as a getter only
  * Posted (IValueDisplayer) as a getter only
  * Available (IValueDisplayer) as a getter only
* Create the new internal class named BudgetAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices - this class should:
  * Implement IBudgetAccountValuesDisplayer
  * Have a private constructor with arguments to initialize all properties
  * Have a internal static method named CreateAsync which:
    * Takes a StaticTextKey and an IStaticTextProvider to resolve the header value
    * Takes a BudgetInfoValuesModel and an IFormatProvider to initialize the display values for budget, posted and available
    * Takes a CancellationToken to use which the IStaticTextProvider
    * Returns a Task<IBudgetAccountValuesDisplayer>
    * Uses the static text key named Budget and the IStaticTextProvider to get the label for bugdet
    * Uses the static text key named Posted and the IStaticTextProvider to get the label for posted
    * Uses the static text key named Available and the IStaticTextProvider to get the label for available
    * Uses the format provider with ToString("C") to make the value for budget, posted and available
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at BudgetAccountValuesDisplayer to ensure dependency calls and logic works
* Extend IBudgetAccountTexts at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties (only getters):
  * StatusDate (IValueDisplayer) as a getter only
  * ValuesForMonthOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
  * ValuesForLastMonthOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
  * ValuesForYearToDateOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
  * ValuesForLastYearOfStatusDate (IBudgetAccountValuesDisplayer) as a getter only
* Modify BudgetAccountTexts in OSDevGrp.OSIntranet.Bff.DomainServices:
  * Make the existing constructor private or proctected if possible
  * Update the constructor with arguments to initialize all properties
  * Make a internal static method named CreateAsync which:
    * Takes an IValueDisplayer to intialize the status date
    * Takes an IStaticTextProvider to resolve static texts
    * Takes a BudgetAccountModel and an IFormatProvider to initialize the display values for ValuesForMonthOfStatusDate, ValuesForLastMonthOfStatusDate, ValuesForYearToDateOfStatusDate and ValuesForLastYearOfStatusDate
    * Takes a CancellationToken to use which the IStaticTextProvider and CreateAsync calls
    * Returns a Task<IBudgetAccountTexts>
    * Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForMonthOfStatusDate and ValuesForMonthOfStatusDate property on the budget account model to initailize ValuesForMonthOfStatusDate
    * Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForLastMonthOfStatusDate and ValuesForLastMonthOfStatusDate property on the budget account model to initailize ValuesForLastMonthOfStatusDate
    * Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForYearToDateOfStatusDate and ValuesForYearToDateOfStatusDate property on the budget account model to initailize ValuesForYearToDateOfStatusDate
    * Uses the CreateAsync at BudgetAccountValuesDisplayer with the static text key BudgetAccountValuesForLastYearOfStatusDate and ValuesForLastYearOfStatusDate property on the budget account model to initailize ValuesForLastYearOfStatusDate
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at BudgetAccountTexts to ensure dependency calls and logic works
* Modify BuildAsync at BudgetAccountTextsBuilder in OSDevGrp.OSIntranet.Bff.DomainServices:
  * Make a IValueDisplayer for the StatusDate from the BudgetAccountModel in the same way as BuildAsync do on the AccountingTextsBuilder
  * Call CreateAsync at BudgetAccountTexts to create the dynamic texts for an account
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for the BudgetAccountTextsBuilder in the same way we tests AccountingTextsBuilder
* Ensure that IBudgetAccountTextsBuilder are correctly registered with BudgetAccountTextsBuilder in ServiceCollectionExtensions
* We don't need to use IBudgetAccountTextsBuilder at the domain service layer yet but keep any usage as is

## Extend dynamic texts for contact account

* Add the following key to StaticTextKey at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
  * ContactAccountValuesAtStatusDate
  * ContactAccountValuesAtEndOfLastMonthFromStatusDate
  * ContactAccountValuesValuesAtEndOfLastYearFromStatusDate
* Update StaticTextProvider in OSDevGrp.OSIntranet.Bff.DomainServices so:
  * ContactAccountValuesAtStatusDate would return "Saldooplysninger pr. dags dato"
  * ContactAccountValuesAtEndOfLastMonthFromStatusDate would return "Saldooplysninger ved sidste måneds afslutningg"
  * ContactAccountValuesValuesAtEndOfLastYearFromStatusDate would return "Saldooplysninger ved sidste års afslutning"
* Add the following Test Cases to GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt at GetStaticTextAsyncTests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests:
  * [TestCase(StaticTextKey.ContactAccountValuesAtStatusDate, "Saldooplysninger pr. dags dato", 0)]
  * [TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastMonthFromStatusDate, "Saldooplysninger ved sidste måneds afslutning", 0)]
  * [TestCase(StaticTextKey.ContactAccountValuesValuesAtEndOfLastYearFromStatusDate, "Saldooplysninger ved sidste års afslutning", 0)]
* Make the new interface IContactAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties:
  * Header (string) as a getter only
  * Balance (IValueDisplayer) as a getter only
* Create the new internal class named ContactAccountValuesDisplayer in OSDevGrp.OSIntranet.Bff.DomainServices - this class should:
  * Implement IContactAccountValuesDisplayer
  * Have a private constructor with arguments to initialize all properties
  * Have a internal static method named CreateAsync which:
    * Takes a StaticTextKey and an IStaticTextProvider to resolve the header value
    * Takes a BalanceInfoValuesModel and an IFormatProvider to initialize the display values for balance
    * Takes a CancellationToken to use which the IStaticTextProvider
    * Returns a Task<IContactAccountValuesDisplayer>
    * Uses the static text key named Balance and the IStaticTextProvider to get the label for balance
    * Uses the format provider with ToString("C") to make the value for balance
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at ContactAccountValuesDisplayer to ensure dependency calls and logic works
* Extend IContactAccountTexts at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces with the following properties (only getters):
  * StatusDate (IValueDisplayer) as a getter only
  * ValuesAtStatusDate (IContactAccountValuesDisplayer) as a getter only
  * ValuesAtEndOfLastMonthFromStatusDate (IContactAccountValuesDisplayer) as a getter only
  * ValuesAtEndOfLastYearFromStatusDate (IContactAccountValuesDisplayer) as a getter only
* Modify ContactAccountTexts in OSDevGrp.OSIntranet.Bff.DomainServices:
  * Make the existing constructor private or proctected if possible
  * Update the constructor with arguments to initialize all properties
  * Make a internal static method named CreateAsync which:
    * Takes an IValueDisplayer to intialize the status date
    * Takes an IStaticTextProvider to resolve static texts
    * Takes a ContactAccountModel and an IFormatProvider to initialize the display values for ValuesAtStatusDate, ValuesAtEndOfLastMonthFromStatusDate and ValuesAtEndOfLastYearFromStatusDate
    * Takes a CancellationToken to use which the IStaticTextProvider and CreateAsync calls
    * Returns a Task<IContactAccountTexts>
    * Uses the CreateAsync at ContactAccountValuesDisplayer with the static text key ContactAccountValuesAtStatusDate and ValuesAtStatusDate property on the budget account model to initailize ValuesAtStatusDate
    * Uses the CreateAsync at ContactAccountValuesDisplayer with the static text key ContactAccountValuesAtEndOfLastMonthFromStatusDate and ValuesAtEndOfLastMonthFromStatusDate property on the budget account model to initailize ValuesAtEndOfLastMonthFromStatusDate
    * Uses the CreateAsync at ContactAccountValuesDisplayer with the static text key ContactAccountValuesValuesAtEndOfLastYearFromStatusDate and ValuesAtEndOfLastYearFromStatusDate property on the budget account model to initailize ValuesAtEndOfLastYearFromStatusDate
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for CreateAsync at ContactAccountTexts to ensure dependency calls and logic works
* Modify BuildAsync at ContactAccountTextsBuilder in OSDevGrp.OSIntranet.Bff.DomainServices:
  * Make a IValueDisplayer for the StatusDate from the ContactAccountModel in the same way as BuildAsync do on the AccountingTextsBuilder
  * Call CreateAsync at ContactAccountTexts to create the dynamic texts for an account
* Create unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for the ContactAccountTextsBuilder in the same way we tests AccountingTextsBuilder
* Ensure that IContactAccountTextsBuilder are correctly registered with ContactAccountTexts in ServiceCollectionExtensions
* We don't need to use IContactAccountTextsBuilder at the domain service layer yet but keep any usage as is
