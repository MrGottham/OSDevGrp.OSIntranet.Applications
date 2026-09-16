# Appending posting journal for an accounting

## General

We need to append/implement functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways
* OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices
* OSDevGrp.OSIntranet.Bff.WebApi
* **OUT OF SCOPE:** osdevgrp.osintranet.react

We need to create tests for functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.WebApi.Tests

**OUT OF SCOPE:** Note: No automated tests are needed for osdevgrp.osintranet.react as the React component will be validated through manual testing.

## Expose logic to add a posting line to a given accounting's posting journal from the BFF WebApi

### Business Goal

The WebApi should expose logic to add a posting line to a given accounting's posting journal. This should support the React application to add a posting line to a given accounting's posting journal then reload the posting journal.

### Implementation

#### PostingJournalLineIdentificationRequestBase (OSDevGrp.OSIntranet.Bff.DomainServices)

- [ ] Create abstract class `PostingJournalLineIdentificationRequestBase` in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/`
- [ ] Inherit from `AccountingIdentificationRequestBase`
- [ ] Add `#region Constructor` section with protected constructor accepting `requestId` (Guid), `accountingNumber` (int), `identifier` (Guid), and `securityContext` (ISecurityContext)
- [ ] Constructor passes `requestId`, `accountingNumber`, and `securityContext` to base class
- [ ] Constructor initializes `Identifier` property
- [ ] Add `#region Properties` section with get-only `Identifier` property (Guid type)

#### PostingJournalLineDataRequestBase (OSDevGrp.OSIntranet.Bff.DomainServices)

- [ ] Create abstract class `PostingJournalLineDataRequestBase` in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/`
- [ ] Inherit from `PostingJournalLineIdentificationRequestBase`
- [ ] Add `#region Constructor` section with protected constructor accepting `requestId` (Guid), `accountingNumber` (int), `identifier` (Guid), `postingDate` (DateTimeOffset), `postingReference` (nullable string), `account` (string), `postingText` (string), `budgetAccount` (nullable string), `debit` (nullable decimal), `credit` (nullable decimal), `contactAccount` (nullable string), and `securityContext` (ISecurityContext)
- [ ] Constructor passes `requestId`, `accountingNumber`, `identifier`, and `securityContext` to base class
- [ ] Constructor initializes all data properties
- [ ] Add `#region Properties` section with get-only properties:
  - [ ] `PostingDate` (DateTimeOffset)
  - [ ] `PostingReference` (nullable string)
  - [ ] `Account` (string)
  - [ ] `PostingText` (string)
  - [ ] `BudgetAccount` (nullable string)
  - [ ] `Debit` (nullable decimal)
  - [ ] `Credit` (nullable decimal)
  - [ ] `ContactAccount` (nullable string)

#### PostingLineFeatureBase (OSDevGrp.OSIntranet.Bff.DomainServices)

- [ ] Create internal abstract class `PostingLineFeatureBase` in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/`
- [ ] Add generic parameter `TPostingJournalLineRequest` constrained to `PostingJournalLineIdentificationRequestBase`
- [ ] Inherit from `AccountingIdentificationFeatureBase<TPostingJournalLineRequest>`
- [ ] Add `#region Constructor` section with protected constructor accepting `permissionChecker` (IPermissionChecker), `accountingGateway` (IAccountingGateway), and `staticTextProvider` (IStaticTextProvider)
- [ ] Constructor passes `permissionChecker` and `accountingGateway` to base class
- [ ] Constructor initializes `StaticTextProvider` property
- [ ] Add `#region Properties` section with protected get-only `StaticTextProvider` property (IStaticTextProvider type)
- [ ] Add `#region Methods` section with sealed override of `ExecuteAsync(TPostingJournalLineRequest request, CancellationToken cancellationToken)`:
  - [ ] Call `AccountingGateway.GetPostingJournalAsync(request.AccountingNumber, cancellationToken)` to retrieve the current posting journal
  - [ ] Call `ProcessPostingJournalAsync(postingJournal, request, cancellationToken)` to process/update the posting journal
  - [ ] Call `AccountingGateway.SavePostingJournalAsync(request.AccountingNumber, postingJournal, cancellationToken)` to persist the updated posting journal
- [ ] Create abstract method `ProcessPostingJournalAsync(ApplyPostingJournalModel postingJournal, TPostingJournalLineRequest request, CancellationToken cancellationToken)` for subclasses to implement

#### ValidationExceptionBase (OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces)

- [ ] Create abstract class `ValidationExceptionBase` in `OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/`
- [ ] Inherit from `Exception`
- [ ] Add `#region Constructors` section with protected constructor accepting `message` (string)
- [ ] Constructor passes `message` to base class via `base(message)`

#### IdentifierExceptionBase (OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces)

- [ ] Create abstract class `IdentifierExceptionBase` in `OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/`
- [ ] Inherit from `ValidationExceptionBase`
- [ ] Add `#region Constructor` section with protected constructor accepting `message` (string) and `identifier` (Guid)
- [ ] Constructor passes `message` to base class
- [ ] Add `#region Properties` section with get-only `Identifier` property (Guid type)
- [ ] Constructor initializes `Identifier` property

#### IdentifierAlreadyExistsException (OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces)

- [ ] Create class `IdentifierAlreadyExistsException` in `OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/`
- [ ] Inherit from `IdentifierExceptionBase`
- [ ] Add `#region Constructors` section with public constructor accepting `identifier` (Guid) and optional `message` (string)
- [ ] Default `message` parameter: `"The identifier already exists."`
- [ ] Constructor passes `message` and `identifier` to base class

#### UnknownIdentifierException (OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces)

- [ ] Create class `UnknownIdentifierException` in `OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/`
- [ ] Inherit from `IdentifierExceptionBase`
- [ ] Add `#region Constructors` section with public constructor accepting `identifier` (Guid) and optional `message` (string)
- [ ] Default `message` parameter: `"The identifier is unknown."`
- [ ] Constructor passes `message` and `identifier` to base class

#### ProblemDetailsFactory Error Handling Updates (OSDevGrp.OSIntranet.Bff.WebApi)

- [ ] Add import: `using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;`
- [ ] Update `ProblemDetailsFactory._problemDetailsBuilders` dictionary:
  - [ ] Replace mapping for `System.Security.VerificationException` with `VerificationFailedException`
  - [ ] Map `VerificationFailedException` to return Bad Request (400) status code: `(httpRequest, exception) => ToProblemDetails(httpRequest, HttpStatusCode.BadRequest, "Bad Request", exception.Message)`
  - [ ] Add mapping for `IdentifierAlreadyExistsException` to return Bad Request (400): `(httpRequest, exception) => ToProblemDetails(httpRequest, HttpStatusCode.BadRequest, "Bad Request", exception.Message)`
  - [ ] Add mapping for `UnknownIdentifierException` to return Bad Request (400): `(httpRequest, exception) => ToProblemDetails(httpRequest, HttpStatusCode.BadRequest, "Bad Request", exception.Message)`

#### StaticTextKey Enum Updates (OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces)

- [ ] Add new enum value `IdentifierAlreadyExists` to `StaticTextKey` enum in appropriate location
- [ ] Add new enum value `UnknownIdentifier` to `StaticTextKey` enum in appropriate location

#### StaticTextProvider Text Entries (OSDevGrp.OSIntranet.Bff.DomainServices)

- [ ] Add to `GenerateStaticTexts()` method:
  - [ ] `staticTexts.Add(StaticTextKey.IdentifierAlreadyExists, "Den angivne identifier eksisterer allerede.");`
  - [ ] `staticTexts.Add(StaticTextKey.UnknownIdentifier, "Den angivne identifier er ukendt.");`

### Tests

#### PostingLineFeatureBase Tests (OSDevGrp.OSIntranet.Bff.DomainServices.Tests)

**ExecuteAsyncTests:**

- [ ] Create test fixture class `ExecuteAsyncTests` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Commands/Accounting/PostingLineFeatureBase/`
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertGetPostingJournalAsyncWasCalledWithCorrectAccountingNumber`
  - [ ] Verify `AccountingGateway.GetPostingJournalAsync` called with `request.AccountingNumber`
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertGetPostingJournalAsyncWasCalledWithGivenCancellationToken`
  - [ ] Verify `AccountingGateway.GetPostingJournalAsync` called with the given cancellation token
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertProcessPostingJournalAsyncWasCalledWithCorrectPostingJournal`
  - [ ] Capture the posting journal returned from `GetPostingJournalAsync`
  - [ ] Verify `ProcessPostingJournalAsync` called with that posting journal
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertProcessPostingJournalAsyncWasCalledWithGivenRequest`
  - [ ] Verify `ProcessPostingJournalAsync` called with the given request
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertProcessPostingJournalAsyncWasCalledWithGivenCancellationToken`
  - [ ] Verify `ProcessPostingJournalAsync` called with the given cancellation token
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertSavePostingJournalAsyncWasCalledWithCorrectAccountingNumber`
  - [ ] Verify `SavePostingJournalAsync` called with `request.AccountingNumber`
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertSavePostingJournalAsyncWasCalledWithModifiedPostingJournal`
  - [ ] Verify `SavePostingJournalAsync` called with the posting journal after processing
- [ ] Add test: `ExecuteAsync_WhenCalled_AssertSavePostingJournalAsyncWasCalledWithGivenCancellationToken`
  - [ ] Verify `SavePostingJournalAsync` called with the given cancellation token
- [ ] Create nested test double class `TestPostingJournalLineRequest` inheriting from `PostingJournalLineIdentificationRequestBase`

#### ProblemDetailsFactory Tests (OSDevGrp.OSIntranet.Bff.WebApi.Tests)

**CreateProblemDetailsTests:**

- [ ] Add import: `using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;`
- [ ] Update test: Rename `CreateProblemDetails_WhenCalledWithVerificationException_ReturnsExpectedProblemDeails()` to `CreateProblemDetails_WhenCalledWithVerificationFailedException_ReturnsExpectedProblemDeails()`
  - [ ] Change exception type from `System.Security.VerificationException` to `VerificationFailedException`
  - [ ] Verify the test still uses correct status code (Bad Request) and message handling
- [ ] Add test: `CreateProblemDetails_WhenCalledWithIdentifierAlreadyExistsException_ReturnsExpectedProblemDeails()`
  - [ ] Create an `IdentifierAlreadyExistsException` with a test identifier
  - [ ] Call `CreateProblemDetails(httpRequest, exception)`
  - [ ] Verify returns BadRequest status code, title "Bad Request", and exception message
  - [ ] Verify request URL is returned in instance
- [ ] Add test: `CreateProblemDetails_WhenCalledWithUnknownIdentifierException_ReturnsExpectedProblemDeails()`
  - [ ] Create an `UnknownIdentifierException` with a test identifier
  - [ ] Call `CreateProblemDetails(httpRequest, exception)`
  - [ ] Verify returns BadRequest status code, title "Bad Request", and exception message
  - [ ] Verify request URL is returned in instance

#### StaticTextProvider Tests (OSDevGrp.OSIntranet.Bff.DomainServices.Tests)

**GetStaticTextAsyncTests:**

- [ ] Add test case: `[TestCase(StaticTextKey.IdentifierAlreadyExists, "Den angivne identifier eksisterer allerede.", 0)]`
  - [ ] Add to the parameterized `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt` test method
  - [ ] Text has 0 format arguments (no placeholders)
- [ ] Add test case: `[TestCase(StaticTextKey.UnknownIdentifier, "Den angivne identifier er ukendt.", 0)]`
  - [ ] Add to the parameterized `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt` test method
  - [ ] Text has 0 format arguments (no placeholders)
