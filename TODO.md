# Appending posting journal for an accounting

## General

We need to append functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.DomainServices
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.WebApi
* osdevgrp.osintranet.react

We need to create tests and test data for functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.WebApi.Tests

Note: No automated tests are needed for osdevgrp.osintranet.react as the React component will be validated through manual testing.

## Making user story to get account, budget acccount and contact account information into the `PostingJournal` component

### Business goal (React Frontend Only)

We need to update the [PostingJournal](osdevgrp.osintranet.react/src/components/PostingJournal.jsx) React component so it can call backend endpoints to fetch account, budget account, and contact account information. The component should display this information when a user enters the corresponding account number and a posting date.

User interactions:

* When user enters an account number: fill out the account name, credit, and available fields
* When user enters a budget account number: fill out the account name, posted, and available fields  
* When user enters a contact account number: fill out the account name and balance fields

If any lookup fails, the component should silently blank out the associated fields (no error messages to user).

### Requirements (React Frontend Only)

#### AccountingService (React Frontend)

* Add `getAccountSummary(accountingNumber, accountNumber, postingDate)` method
  * Calls AccountingController.AccountSummaryAsync endpoint
  * Returns JSON response from API
  * **Endpoint:** `GET /api/accounting/{accountingNumber}/accounts/{accountNumber}/summary?statusDate={statusDate}`
  * **Response:** AccountSummaryResponseDto with:
    * `statusDate`: ValueDisplayerDto (label, value)
    * `valuesAtStatusDate`: AccountValuesDisplayerDto (header, credit, balance, available)
    * `valuesAtEndOfLastMonthFromStatusDate`: AccountValuesDisplayerDto
    * `valuesAtEndOfLastYearFromStatusDate`: AccountValuesDisplayerDto
    * `accountName`: string (max 256 chars)
    * `accounting`: AccountingIdentificationDto (number)
    * `accountNumber`: string (1-16 chars, pattern: `^[0-9A-ZÆØÅ\-+]{1,16}$`)

* Add `getBudgetAccountSummary(accountingNumber, budgetAccountNumber, postingDate)` method
  * Calls AccountingController.BudgetAccountSummaryAsync endpoint
  * Returns JSON response from API
  * **Endpoint:** `GET /api/accounting/{accountingNumber}/budgetaccounts/{budgetAccountNumber}/summary?statusDate={statusDate}`
  * **Response:** BudgetAccountSummaryResponseDto with:
    * `statusDate`: ValueDisplayerDto (label, value)
    * `valuesForMonthOfStatusDate`: BudgetAccountValuesDisplayerDto (header, budget, posted, available)
    * `valuesForLastMonthOfStatusDate`: BudgetAccountValuesDisplayerDto
    * `valuesForYearToDateOfStatusDate`: BudgetAccountValuesDisplayerDto
    * `valuesForLastYearOfStatusDate`: BudgetAccountValuesDisplayerDto
    * `accountName`: string (max 256 chars)
    * `accounting`: AccountingIdentificationDto (number)
    * `accountNumber`: string (1-16 chars, pattern: `^[0-9A-ZÆØÅ\-+]{1,16}$`)

* Add `getContactAccountSummary(accountingNumber, contactAccountNumber, postingDate)` method
  * Calls AccountingController.ContactAccountSummaryAsync endpoint
  * Returns JSON response from API
  * **Endpoint:** `GET /api/accounting/{accountingNumber}/contactaccounts/{contactAccountNumber}/summary?statusDate={statusDate}`
  * **Response:** ContactAccountSummaryResponseDto with:
    * `statusDate`: ValueDisplayerDto (label, value)
    * `valuesAtStatusDate`: ContactAccountValuesDisplayerDto (header, balance)
    * `valuesAtEndOfLastMonthFromStatusDate`: ContactAccountValuesDisplayerDto
    * `valuesAtEndOfLastYearFromStatusDate`: ContactAccountValuesDisplayerDto
    * `accountName`: string (max 256 chars)
    * `accounting`: AccountingIdentificationDto (number)
    * `accountNumber`: string (1-16 chars, pattern: `^[0-9A-ZÆØÅ\-+]{1,16}$`)

* Validate all three methods require: accountingNumber, accountNumber (or variant), and postingDate (mapped to statusDate query param)
* Implement consistent error handling: throw errors for invalid params, throw from response on failure
* Follow existing method patterns (parameter validation, fetch with credentials, response.ok checks, error generation)
* Use consistent naming: `getSummary*()` method naming matches existing `getAccountingSummary()` pattern

#### PostingJournal Component (React Frontend)

* Retrieve `AccountingService` from `ServiceContext` using dependency injection pattern
  * Add import at top: `import { ServiceContext } from '../contexts/ServiceContext';`
  * Add after line 23: `const accountingService = useContext(ServiceContext).accountingService;`
  * ServiceContext provides: homeService, accountingService, authenticateService, securityService

* Complete the `populateAccountDetails()` callback implementation (lines 57-76)
  * Parameter validation already in place (lines 64-73: accountingNumber, accountNumber trim/empty check, postingDate)
  * After validation passes, convert postingDate to ISO string: `dateHelper.convertToIsoString(formData.postingDate)`
  * Call: `accountingService.getAccountSummary(accountingNumber, formData.accountNumber, isoFormattedDate)`
  * Wrap call in try/catch; on success extract and populate:
    * `computedData.account.name` from response `accountName`
    * `computedData.account.credit` from response `valuesAtStatusDate.credit.value`
    * `computedData.account.available` from response `valuesAtStatusDate.available.value`
  * On error (404, 500, timeout, network, or any other error): catch error, log to console: `console.log('Account lookup failed:', error);`, fields remain `undefined` (already blanked on line 60)
  * Replace existing console.debug calls (lines 74-76) with actual service call

* Complete the `populateBudgetAccountDetails()` callback implementation (lines 77-96)
  * Parameter validation already in place (lines 84-93: accountingNumber, budgetAccountNumber trim/empty check, postingDate)
  * After validation passes, convert postingDate to ISO string: `dateHelper.convertToIsoString(formData.postingDate)`
  * Call: `accountingService.getBudgetAccountSummary(accountingNumber, formData.budgetAccountNumber, isoFormattedDate)`
  * Wrap call in try/catch; on success extract and populate:
    * `computedData.budgetAccount.name` from response `accountName`
    * `computedData.budgetAccount.posted` from response `valuesForMonthOfStatusDate.posted.value`
    * `computedData.budgetAccount.available` from response `valuesForMonthOfStatusDate.available.value`
  * On error (404, 500, timeout, network, or any other error): catch error, log to console: `console.log('Budget account lookup failed:', error);`, fields remain `undefined` (already blanked on line 80)
  * Replace existing console.debug calls (lines 94-96) with actual service call

* Complete the `populateContactAccountDetails()` callback implementation (lines 97-116)
  * Parameter validation already in place (lines 104-113: accountingNumber, contactAccountNumber trim/empty check, postingDate)
  * After validation passes, convert postingDate to ISO string: `dateHelper.convertToIsoString(formData.postingDate)`
  * Call: `accountingService.getContactAccountSummary(accountingNumber, formData.contactAccountNumber, isoFormattedDate)`
  * Wrap call in try/catch; on success extract and populate:
    * `computedData.contactAccount.name` from response `accountName`
    * `computedData.contactAccount.balance` from response `valuesAtStatusDate.balance.value`
  * On error (404, 500, timeout, network, or any other error): catch error, log to console: `console.log('Contact account lookup failed:', error);`, fields remain `undefined` (already blanked on line 100)
  * Replace existing console.debug calls (lines 114-116) with actual service call

* Note: Async state transitions already implemented (lines 120-157)
  * useEffect hooks wrap populate calls with `startAccountTransition()`, `startBudgetAccountTransition()`, `startContactAccountTransition()`
  * Transitions update `isAccountPending`, `isBudgetAccountPending`, `isContactAccountPending` flags
  * All three operations (account, budget, contact lookups) run independently and can complete in any order
  
* Note: Input field `readOnly` states already implemented based on pending flags
  * Example: line 268 has `readOnly={isAccountPending}` on account number field
  * Budget account input: line 309 has `readOnly={isBudgetAccountPending}`
  * Contact account input: line 351 has `readOnly={isContactAccountPending}`
