# PRD: PostingJournal Account Lookup Implementation

## Problem

The PostingJournal component currently has three placeholder callback functions (`populateAccountDetails`, `populateBudgetAccountDetails`, `populateContactAccountDetails`) that validate input parameters and log to console, but perform no actual account lookups. Users cannot verify account details (names, balances, availability) when entering posting journal lines, forcing manual verification via separate lookups — slowing data entry and increasing error risk.

Backend API endpoints for account lookups already exist and are functional.

## Relevant Codebase

### PostingJournal Component
**File:** [osdevgrp.osintranet.react/src/components/PostingJournal.jsx](osdevgrp.osintranet.react/src/components/PostingJournal.jsx)

**Current state:**
- Three placeholder callbacks (lines 47–121) that validate params and log to console only
- Pending state management already in place via `useTransition()` hooks (lines 42–44):
  - `isAccountPending`, `isBudgetAccountPending`, `isContactAccountPending`
  - Each wrapped in corresponding `startAccountTransition()`, `startBudgetAccountTransition()`, `startContactAccountTransition()`
- Form field `readOnly` attributes already wired to pending flags (e.g., line 268: `readOnly={isAccountPending}`)
- 750ms debounce already implemented (line 35: `const changeTimeout = 750`) with ref-based timers (lines 266–277)
- Three sets of display-only form fields already rendered (lines 268–350):
  - Account: name, credit, available
  - Budget account: name, posted, available
  - Contact account: name, balance
- `computedData` state already defined with correct structure (lines 28–32)
- Three useEffect hooks trigger lookups when inputs change (lines 124–152)

**To implement:**
- Import `ServiceContext` to access AccountingService
- Import Bootstrap `Toast` and `ToastContainer` for error display
- Add toast state management: `useState([])` to track active toasts
- Implement `addToast(header, body)` utility with auto-dismiss (5 seconds) and manual close
- Replace placeholder logic in three callbacks with: service call + error handling + field population

### AccountingService
**File:** [osdevgrp.osintranet.react/src/services/AccountingService.jsx](osdevgrp.osintranet.react/src/services/AccountingService.jsx)

**Current pattern** (lines 3–39):
```javascript
async getAccountingPreCreation() {
    const response = await fetch(this.resolveEndpoint('/api/accounting/create'), { credentials: 'include' });
    if (response.ok) {
        return await response.json();
    }
    throw await this.generateError(response);
}
```

**To add:** Three new methods following this pattern, each with:
- Parameter validation (non-null/non-empty checks)
- Date-to-ISO-string conversion via new `DateHelper.convertToIsoString()` method
- Fetch with `credentials: 'include'`
- Response handling: return JSON on success, throw error on failure
- Error thrown via `this.generateError(response)` (already extracts detail → title → statusText)

### DateHelper
**File:** [osdevgrp.osintranet.react/src/helpers/DateHelper.jsx](osdevgrp.osintranet.react/src/helpers/DateHelper.jsx)

**Current state** (full file):
```javascript
export default class DateHelper {
    getCurrentDate() {
        return this.getDateOnly(new Date());
    }

    getDateOnly(date) {
        const dateOnly = new Date(date);
        dateOnly.setHours(0, 0, 0, 0);
        return dateOnly;
    }
}
```

**To add:** `convertToIsoString(date)` method that:
- Validates date is not null/undefined
- Validates date is a Date object
- Returns ISO 8601 string via `date.toISOString()`
- Throws descriptive Error on validation failure

### ServiceBase
**File:** [osdevgrp.osintranet.react/src/services/ServiceBase.jsx](osdevgrp.osintranet.react/src/services/ServiceBase.jsx)

**Verified:** `generateError(response)` and `problemDetailsToError(problemDetails, fallbackMessage)` already handle:
- HTTP 400, 401, 500 responses
- ProblemDetails extraction with priority: detail → title → statusText
- Returns Error object compatible with catch blocks
- No changes needed

### ServiceContext
**File:** [osdevgrp.osintranet.react/src/contexts/ServiceContext.jsx](osdevgrp.osintranet.react/src/contexts/ServiceContext.jsx)

**Current state:** Instantiates AccountingService as singleton; no changes needed.

### Backend API Endpoints
All three endpoints verified to exist and return ProblemDetails on error:

1. **Account Lookup**
   - Endpoint: `GET /api/accounting/{accountingNumber}/accounts/{accountNumber}/summary?statusDate={statusDate}`
   - Success response includes: `accountName`, `valuesAtStatusDate.credit.value`, `valuesAtStatusDate.available.value`
   - Error responses: HTTP 400, 401, or 500 with ProblemDetails

2. **Budget Account Lookup**
   - Endpoint: `GET /api/accounting/{accountingNumber}/budgetaccounts/{budgetAccountNumber}/summary?statusDate={statusDate}`
   - Success response includes: `accountName`, `valuesForMonthOfStatusDate.posted.value`, `valuesForMonthOfStatusDate.available.value`

3. **Contact Account Lookup**
   - Endpoint: `GET /api/accounting/{accountingNumber}/contactaccounts/{contactAccountNumber}/summary?statusDate={statusDate}`
   - Success response includes: `accountName`, `valuesAtStatusDate.balance.value`

## Goal

Enable real-time account details auto-population in the PostingJournal component. When a user enters an account number and moves focus away (after 750ms debounce), the component calls the backend API to fetch and display:
- Account name, credit balance, and available amount (for standard accounts)
- Budget account name, posted amount, and available amount (for budget accounts)
- Contact account name and balance (for contact accounts)

On any API error (HTTP 400/401/500, network timeout, etc.), display a user-friendly Bootstrap toast with the error message. All three lookups operate independently and may complete in any order.

## User Stories

**Story 1: Account Lookup on Posting**
As a user entering a posting journal line in the accounting system, I want the component to automatically look up and display the account name and balance data when I enter an account number, so I can verify I'm posting to the correct account without manually navigating away to check account details.

**Story 2: Budget Account Verification**
As a user creating a posting journal entry, I want to see the budget account name and posted/available amounts auto-populated when I enter a budget account number, so I can ensure I'm posting within budget limits before submitting.

**Story 3: Contact Account Balance Check**
As a user recording transactions with contact accounts (debtors/creditors), I want the component to fetch and display the contact account name and current balance, so I can verify the account status before posting.

**Story 4: Clear Error Feedback**
As a user, if an account lookup fails (invalid account, network error, server error), I want to see a clear error message in a toast notification, so I understand what went wrong and can take corrective action.

## Acceptance Criteria

### AC1: Account Number Lookup

**Given** a user enters a valid account number in the account number field and moves focus away (after 750ms debounce)  
**When** the PostingJournal component completes the lookup  
**Then:**
- [ ] Component calls `accountingService.getAccountSummary(accountingNumber, formData.accountNumber, isoDateString)`
- [ ] On HTTP 200 response:
  - [ ] `computedData.account.name` is set from response field `accountName`
  - [ ] `computedData.account.credit` is set from response field `valuesAtStatusDate.credit.value`
  - [ ] `computedData.account.available` is set from response field `valuesAtStatusDate.available.value`
  - [ ] Account number input field remains read-only (via `isAccountPending` flag) until lookup completes
  - [ ] Fields display populated values in UI (read-only form controls)
- [ ] On API error (HTTP 400, 401, 500, network, or timeout):
  - [ ] Bootstrap Toast displayed with header "Account Lookup Failed" and body containing error message (extracted from response.detail or response.title or statusText)
  - [ ] Toast variant: "warning", position: "top-end", auto-dismisses after 5 seconds
  - [ ] Associated fields (`name`, `credit`, `available`) remain undefined/blank in UI
  - [ ] Account number input field becomes editable again (pending flag cleared)
  - [ ] User can retry by modifying account number and waiting 750ms

### AC2: Budget Account Number Lookup

**Given** a user enters a valid budget account number in the budget account number field and moves focus away (after 750ms debounce)  
**When** the PostingJournal component completes the lookup  
**Then:**
- [ ] Component calls `accountingService.getBudgetAccountSummary(accountingNumber, formData.budgetAccountNumber, isoDateString)`
- [ ] On HTTP 200 response:
  - [ ] `computedData.budgetAccount.name` is set from response field `accountName`
  - [ ] `computedData.budgetAccount.posted` is set from response field `valuesForMonthOfStatusDate.posted.value`
  - [ ] `computedData.budgetAccount.available` is set from response field `valuesForMonthOfStatusDate.available.value`
  - [ ] Budget account number input field remains read-only (via `isBudgetAccountPending` flag) until lookup completes
- [ ] On API error (HTTP 400, 401, 500, network, or timeout):
  - [ ] Bootstrap Toast displayed with header "Budget Account Lookup Failed" and error message body
  - [ ] Toast variant: "warning", position: "top-end", auto-dismisses after 5 seconds
  - [ ] Associated fields (`name`, `posted`, `available`) remain undefined/blank
  - [ ] Budget account number input field becomes editable again
  - [ ] User can retry by modifying budget account number

### AC3: Contact Account Number Lookup

**Given** a user enters a valid contact account number in the contact account number field and moves focus away (after 750ms debounce)  
**When** the PostingJournal component completes the lookup  
**Then:**
- [ ] Component calls `accountingService.getContactAccountSummary(accountingNumber, formData.contactAccountNumber, isoDateString)`
- [ ] On HTTP 200 response:
  - [ ] `computedData.contactAccount.name` is set from response field `accountName`
  - [ ] `computedData.contactAccount.balance` is set from response field `valuesAtStatusDate.balance.value`
  - [ ] Contact account number input field remains read-only (via `isContactAccountPending` flag) until lookup completes
- [ ] On API error (HTTP 400, 401, 500, network, or timeout):
  - [ ] Bootstrap Toast displayed with header "Contact Account Lookup Failed" and error message body
  - [ ] Toast variant: "warning", position: "top-end", auto-dismisses after 5 seconds
  - [ ] Associated fields (`name`, `balance`) remain undefined/blank
  - [ ] Contact account number input field becomes editable again
  - [ ] User can retry by modifying contact account number

### AC4: Parameter Validation

**Given** any account lookup is triggered  
**When** parameters are validated before API call  
**Then:**
- [ ] `accountingNumber` must be present and non-empty after trim
- [ ] Account number variant (`accountNumber`, `budgetAccountNumber`, or `contactAccountNumber`) must be present and non-empty after trim
- [ ] `postingDate` must be a valid Date object (not null/undefined)
- [ ] If any validation fails, the lookup is silently skipped (no API call, no toast, no error)
- [ ] If validation passes, date is converted to ISO 8601 string format (e.g., "2026-08-11T00:00:00.000Z") before service call

### AC5: DateHelper.convertToIsoString() Implementation

**Given** the DateHelper class needs a new date conversion method  
**Then:**
- [ ] New method: `convertToIsoString(date)`
- [ ] Validates `date` is not null or undefined
- [ ] Validates `date` is a Date object
- [ ] Returns ISO 8601 string via `date.toISOString()`
- [ ] Throws descriptive Error if validation fails (e.g., "Date must be a valid Date object")

### AC6: AccountingService Lookup Methods Implementation

**Given** three new lookup methods need to be added to AccountingService  
**Then:**

#### Method: `getAccountSummary(accountingNumber, accountNumber, postingDate)`
- [ ] Validates `accountingNumber` is not null/undefined/empty after trim
- [ ] Validates `accountNumber` is not null/undefined/empty after trim
- [ ] Validates `postingDate` is a valid Date object
- [ ] Converts `postingDate` to ISO string via `this.#dateHelper.convertToIsoString(postingDate)`
- [ ] Constructs URL: `{endpoint}/api/accounting/{accountingNumber}/accounts/{accountNumber}/summary?statusDate={ISO string}`
- [ ] Fetches with method 'GET', `credentials: 'include'`, standard headers via `this.generateContentTypeHeaderForJson()`
- [ ] Returns parsed JSON on `response.ok`
- [ ] Throws error via `this.generateError(response)` on any error
- [ ] Error message automatically prioritizes: detail → title → statusText (via ServiceBase)

#### Method: `getBudgetAccountSummary(accountingNumber, budgetAccountNumber, postingDate)`
- [ ] Same pattern as `getAccountSummary()` with endpoint: `{endpoint}/api/accounting/{accountingNumber}/budgetaccounts/{budgetAccountNumber}/summary?statusDate={ISO string}`

#### Method: `getContactAccountSummary(accountingNumber, contactAccountNumber, postingDate)`
- [ ] Same pattern as `getAccountSummary()` with endpoint: `{endpoint}/api/accounting/{accountingNumber}/contactaccounts/{contactAccountNumber}/summary?statusDate={ISO string}`

**Setup in AccountingService:**
- [ ] Import DateHelper: `import DateHelper from '../helpers/DateHelper';`
- [ ] Instantiate private field: `#dateHelper = new DateHelper();`

### AC7: PostingJournal Component Integration

**Given** the PostingJournal component needs account lookup callbacks with error display  
**Then:**

#### Imports and Setup
- [ ] Import: `import { ServiceContext } from '../contexts/ServiceContext';`
- [ ] Import: `import { Toast, ToastContainer } from 'react-bootstrap';`
- [ ] Retrieve service: `const accountingService = useContext(ServiceContext).accountingService;`
- [ ] Retrieve helper: `const dateHelper = useContext(HelperContext).dateHelper;` (already exists on line 20)
- [ ] Add toast state: `const [toasts, setToasts] = useState([]);`
- [ ] Implement `addToast(header, body)` function:
  - [ ] Generates unique toastId (e.g., `Date.now() + Math.random()`)
  - [ ] Adds toast object to state: `{ toastId, header, body }`
  - [ ] Auto-dismiss: `setTimeout(() => removeToast(toastId), 5000)`
  - [ ] Provides `removeToast(toastId)` to remove from state array

#### Toast Rendering
- [ ] Render `<ToastContainer position="top-end" className="p-3">` in JSX
- [ ] Map `toasts` array with `.map(toast => <Toast key={toast.toastId} ... />`
- [ ] Each Toast:
  - [ ] Variant: "warning"
  - [ ] Header: toast.header (e.g., "Account Lookup Failed")
  - [ ] Body: toast.body (error message from `error.message`)
  - [ ] On close: remove from state via `removeToast()`

#### populateAccountDetails() Implementation
- [ ] Clear account fields: `setComputedData(prev => ({...prev, account: { name: undefined, credit: undefined, available: undefined }}))`
- [ ] Validate parameters (skip if invalid, no toast):
  - [ ] `accountingNumber` non-empty after trim
  - [ ] `formData.accountNumber` non-empty after trim
  - [ ] `formData.postingDate` is valid Date
- [ ] Convert date: `const isoDate = dateHelper.convertToIsoString(formData.postingDate);`
- [ ] Try/catch wrapping service call:
  - [ ] **Success:** Extract and update:
    - [ ] `computedData.account.name = response.accountName`
    - [ ] `computedData.account.credit = response.valuesAtStatusDate.credit.value`
    - [ ] `computedData.account.available = response.valuesAtStatusDate.available.value`
  - [ ] **Error:** Call `addToast("Account Lookup Failed", error.message)`

#### populateBudgetAccountDetails() Implementation
- [ ] Clear budget account fields: `setComputedData(prev => ({...prev, budgetAccount: { name: undefined, posted: undefined, available: undefined }}))`
- [ ] Validate parameters (skip if invalid):
  - [ ] `accountingNumber` non-empty
  - [ ] `formData.budgetAccountNumber` non-empty
  - [ ] `formData.postingDate` valid Date
- [ ] Convert date: `const isoDate = dateHelper.convertToIsoString(formData.postingDate);`
- [ ] Try/catch wrapping service call:
  - [ ] **Success:** Extract and update:
    - [ ] `computedData.budgetAccount.name = response.accountName`
    - [ ] `computedData.budgetAccount.posted = response.valuesForMonthOfStatusDate.posted.value`
    - [ ] `computedData.budgetAccount.available = response.valuesForMonthOfStatusDate.available.value`
  - [ ] **Error:** Call `addToast("Budget Account Lookup Failed", error.message)`

#### populateContactAccountDetails() Implementation
- [ ] Clear contact account fields: `setComputedData(prev => ({...prev, contactAccount: { name: undefined, balance: undefined }}))`
- [ ] Validate parameters (skip if invalid):
  - [ ] `accountingNumber` non-empty
  - [ ] `formData.contactAccountNumber` non-empty
  - [ ] `formData.postingDate` valid Date
- [ ] Convert date: `const isoDate = dateHelper.convertToIsoString(formData.postingDate);`
- [ ] Try/catch wrapping service call:
  - [ ] **Success:** Extract and update:
    - [ ] `computedData.contactAccount.name = response.accountName`
    - [ ] `computedData.contactAccount.balance = response.valuesAtStatusDate.balance.value`
  - [ ] **Error:** Call `addToast("Contact Account Lookup Failed", error.message)`

#### Existing Behavior (Preserved)
- [ ] `useTransition()` hooks remain unchanged — each callback wrapped in corresponding `startXxxTransition()`
- [ ] Pending flags (`isAccountPending`, `isBudgetAccountPending`, `isContactAccountPending`) continue gating field readonly attributes
- [ ] 750ms debounce timing unchanged
- [ ] All three operations independent, complete in any order
- [ ] No breaking changes to form validation, modal behavior, or submit logic

### AC8: Toast Behavior - Multiple Simultaneous Errors

**Given** multiple account lookups fail at the same time (e.g., all three account types invalid)  
**When** the user triggers lookups for account, budget account, and contact account simultaneously  
**Then:**
- [ ] All three error toasts are displayed simultaneously in a stacked layout (top-end position)
- [ ] Each toast independently auto-dismisses after 5 seconds
- [ ] User can manually close any toast via close button
- [ ] All toasts remain visible until dismissed (either auto or manual)

## Scope

### In Scope
- DateHelper.convertToIsoString() method implementation
- Three new AccountingService lookup methods (getAccountSummary, getBudgetAccountSummary, getContactAccountSummary)
- PostingJournal component integration:
  - ServiceContext and Toast imports
  - Toast state management with auto-dismiss and manual close
  - Implementation of three populate callbacks with service calls and error handling
- Bootstrap Toast error display with configurable header, body, variant, position, auto-dismiss
- ISO 8601 date string formatting for API query parameters
- Parameter validation before API calls (silent skip on failure)

### Out of Scope
- Backend API endpoint implementation (endpoints already exist and are functional)
- Database queries for account lookups (backend responsibility)
- Posting journal line CRUD operations (create/update/delete — separate story)
- Form submission and persistence logic
- Client-side input validation rules (FormHelper/ValidationSchemaHelper handle this)
- Account data caching or request deduplication (each lookup is fresh)
- Server-side timeout configuration (API responses expected within 10 seconds)

## Risks

**None identified.** All required infrastructure exists:
- Pending state management and readonly field gating already implemented
- Debounce timing (750ms) already established
- Error extraction via ServiceBase.generateError() verified to work correctly
- API endpoints verified to exist and return correct response structures
- Bootstrap library already in use
- No new npm dependencies required
- No breaking changes to existing components

---

## Implementation Notes

### Date String Format
- Input: JavaScript Date object from Formik date picker (line 209-211 in PostingJournal.jsx shows date is stored as Date object)
- Processing: Convert via `dateHelper.convertToIsoString(date)` → ISO 8601 string format
- Transport: ISO string sent as `statusDate` query parameter
- Example: `new Date('2026-08-11').toISOString()` → `"2026-08-11T00:00:00.000Z"`

### Error Message Extraction
ServiceBase.generateError() handles ProblemDetails extraction automatically:
- Response structure: `{ detail: "...", title: "...", status: 400, ... }`
- Extraction priority: `response.detail` (most specific) → `response.title` → `response.statusText` (fallback)
- Returned as Error object with message already set
- In catch block, use `error.message` directly — no re-extraction needed

### Toast State Management Pattern
```javascript
const [toasts, setToasts] = useState([]);

const addToast = (header, body) => {
  const toastId = Date.now() + Math.random();
  setToasts(prev => [...prev, { toastId, header, body }]);
  setTimeout(() => removeToast(toastId), 5000); // Auto-dismiss after 5 seconds
};

const removeToast = (toastId) => {
  setToasts(prev => prev.filter(t => t.toastId !== toastId));
};
```

### Async State Transitions
Existing `useTransition()` hooks already handle pending states. Each populate callback wrapped:
```javascript
startAccountTransition(async () => {
  await populateAccountDetails();
});
```
No additional state management required — readonly fields prevent concurrent requests.
