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

### Implementation Bullets

#### OSDevGrp.OSIntranet.Bff.DomainServices
- [ ] Create folder: `Features/Commands/Accounting/AppendPostingLineToPostingJournal`
- [ ] Create `AppendPostingLineToPostingJournalRequest.cs` public class inheriting from `PostingJournalLineDataRequestBase`
- [ ] Implement constructor passing all parameters to base class
- [ ] Create `AppendPostingLineToPostingJournalFeature.cs` internal class inheriting from `PostingLineFeatureBase<AppendPostingLineToPostingJournalRequest>`
- [ ] Implement `ProcessPostingJournalAsync` method with the following logic:
  - [ ] Check if posting journal already contains a posting line with same identifier as request
    - [ ] If identifier exists, throw `IdentifierAlreadyExistsException` with identifier from request and message from `StaticTextProvider.GetStaticTextAsync(StaticTextKey.IdentifierAlreadyExists, ...)`
  - [ ] Create new `ApplyPostingLineModel` with data from request (calculate SortOrder as: max(existing) + 1 or 1)
  - [ ] Add new line to collection of existing lines
  - [ ] Return cloned posting journal via `SortAndClonePostingJournal` method

#### OSDevGrp.OSIntranet.Bff.DomainServices (PostingLineFeatureBase)
- [ ] Add protected method `SortAndClonePostingJournal` to `PostingLineFeatureBase`:
  - [ ] Takes existing `ApplyPostingJournalModel` and `IReadOnlyCollection<ApplyPostingLineModel>`
  - [ ] Clones the journal with sorted posting lines
  - [ ] Sorts by PostingDate (descending), then by SortOrder (descending within same date)
  - [ ] Returns new cloned `ApplyPostingJournalModel`

#### OSDevGrp.OSIntranet.Bff.DomainServices.Tests
- [ ] Create test class: `Features/Commands/Accounting/AppendPostingLineToPostingJournal/ExecuteAsyncTests.cs`
- [ ] Test duplicate identifier detection:
  - [ ] When posting journal contains a line with same identifier as request, throw `IdentifierAlreadyExistsException`
  - [ ] Exception message retrieved from `StaticTextProvider.GetStaticTextAsync(StaticTextKey.IdentifierAlreadyExists, ...)`
  - [ ] Exception includes the identifier from request
- [ ] Test new posting line creation:
  - [ ] `ApplyPostingLineModel` created with all properties mapped from request
  - [ ] Properties include: PostingDate, Reference, AccountNumber, Details, BudgetAccountNumber, Debit, Credit, ContactAccountNumber
  - [ ] Identifier property set from request Identifier
- [ ] Test SortOrder calculation:
  - [ ] When journal has no existing lines, new line SortOrder = 1
  - [ ] When journal has existing lines, new line SortOrder = max(existing SortOrder) + 1
- [ ] Test line collection management:
  - [ ] New line added to collection of existing lines
  - [ ] Collection passed to `SortAndClonePostingJournal`
- [ ] Test cloned journal return:
  - [ ] Returned model is different instance from input (cloned)
  - [ ] Returned model contains sorted lines
  - [ ] Returned model has accounting number preserved

#### OSDevGrp.OSIntranet.Bff.WebApi (DTOs)
- [ ] Create `PostingJournalLineModifierDtoBase.cs` public abstract class in folder `Controllers/Accounting/Dtos/`:
  - [ ] Base class for all posting line modification DTOs (append, update, delete)
  - [ ] Add properties:
    - [ ] `PostingDate` ([Required] required, DateTimeOffset)
    - [ ] `PostingReference` ([MinLength(PostingReferenceMinLength)] [MaxLength(PostingReferenceMaxLength)], optional string)
    - [ ] `Account` ([Required] [MinLength(AccountNumberMinLength)] [MaxLength(AccountNumberMaxLength)] [RegularExpression(AccountNumberRegexPattern)] required, string)
    - [ ] `PostingText` ([Required] [MinLength(PostingTextMinLength)] [MaxLength(PostingTextMaxLength)] required, string)
    - [ ] `BudgetAccount` ([MinLength(AccountNumberMinLength)] [MaxLength(AccountNumberMaxLength)] [RegularExpression(AccountNumberRegexPattern)], optional string)
    - [ ] `Debit` ([Range(DebitMinValue, DebitMaxValue)], optional decimal)
    - [ ] `Credit` ([Range(CreditMinValue, CreditMaxValue)], optional decimal)
    - [ ] `ContactAccount` ([MinLength(AccountNumberMinLength)] [MaxLength(AccountNumberMaxLength)] [RegularExpression(AccountNumberRegexPattern)], optional string)
- [ ] Create `AppendPostingLineToPostingJournalDto.cs` public class inheriting from `PostingJournalLineModifierDtoBase` in folder `Controllers/Accounting/Dtos/`:
  - [ ] Add `Identifier` property ([Required] required, Guid)

#### OSDevGrp.OSIntranet.Bff.WebApi (Endpoint)
- [ ] Create endpoint `AppendPostingLineToPostingJournalAsync` in `AccountingController`:
  - [ ] Route: `[HttpPost("{accountingNumber:int}/postingjournal/postinglines")]`
  - [ ] Authorization: `[Authorize(Policy = Policies.AccountingModifier)]`
  - [ ] Parameters:
    - [ ] `ICommandFeature<AppendPostingLineToPostingJournalDto> commandFeature` ([FromServices])
    - [ ] `IQueryFeature<PostingJournalRequest, PostingJournalResponse> queryFeature` ([FromServices])
    - [ ] `int accountingNumber` ([FromRoute] [Required] [Range(AccountingNumberMinValue, AccountingNumberMaxValue)])
    - [ ] `AppendPostingLineToPostingJournalDto dto` ([FromBody] [Required] required)
    - [ ] `CancellationToken cancellationToken`
  - [ ] Response Types:
    - [ ] `[ProducesResponseType(typeof(PostingJournalResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]`
    - [ ] `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]`
    - [ ] `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]`
    - [ ] `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]`
  - [ ] Implementation logic:
    - [ ] Get security context via `_securityContextProvider.GetCurrentSecurityContextAsync`
    - [ ] Create `AppendPostingLineToPostingJournalRequest` with dto properties, accountingNumber, and security context
    - [ ] Execute command feature with the request
    - [ ] Create `PostingJournalRequest` with accountingNumber, current date status, formatProvider, and security context
    - [ ] Execute query feature with the request
    - [ ] Map result using `PostingJournalResponseDto.Map` and return OK

#### OSDevGrp.OSIntranet.Bff.WebApi.Tests
- [ ] Create test class: `Controllers/AccountingController/AppendPostingLineToPostingJournalAsyncTests.cs`
- [ ] Setup test fixtures with mocks:
  - [ ] `Mock<ICommandFeature<AppendPostingLineToPostingJournalDto>> _commandFeatureMock`
  - [ ] `Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>> _queryFeatureMock`
  - [ ] `Mock<ISecurityContextProvider> _securityContextProviderMock`
  - [ ] `Fixture _fixture`
- [ ] Test command feature execution:
  - [ ] Assert `ExecuteAsync` called on `commandFeature` exactly once
  - [ ] Verify request argument is `AppendPostingLineToPostingJournalRequest` with correct properties:
    - [ ] `AccountingNumber` matches route parameter
    - [ ] `Identifier` matches dto Identifier
    - [ ] `PostingDate` matches dto PostingDate
    - [ ] `PostingReference` matches dto PostingReference
    - [ ] `Account` matches dto Account
    - [ ] `PostingText` matches dto PostingText
    - [ ] `BudgetAccount` matches dto BudgetAccount
    - [ ] `Debit` matches dto Debit
    - [ ] `Credit` matches dto Credit
    - [ ] `ContactAccount` matches dto ContactAccount
    - [ ] Security context from `_securityContextProvider.GetCurrentSecurityContextAsync`
- [ ] Test query feature execution:
  - [ ] Assert `ExecuteAsync` called on `queryFeature` exactly once
  - [ ] Verify request argument is `PostingJournalRequest` with correct properties:
    - [ ] `AccountingNumber` matches route parameter
    - [ ] Security context from `_securityContextProvider.GetCurrentSecurityContextAsync`
    - [ ] Date status reflects current date
    - [ ] Format provider is correctly initialized
- [ ] Test security context provider:
  - [ ] Assert `GetCurrentSecurityContextAsync` called on security context provider exactly once
- [ ] Test response mapping:
  - [ ] Assert returns `OkObjectResult`
  - [ ] Assert value is `PostingJournalResponseDto` mapped from query response
- [ ] Test integration:
  - [ ] Full flow from dto to command execution to query execution to response mapping
