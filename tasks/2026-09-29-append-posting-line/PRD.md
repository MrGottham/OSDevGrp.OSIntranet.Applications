# PRD: Append Posting Line to Posting Journal

## Problem

The React application needs to add posting lines to an accounting's posting journal and immediately display the updated journal state. Today there is no endpoint to perform this operation. Users must manually navigate elsewhere to see their changes reflected.

## Relevant Codebase

### Domain Models
- **ApplyPostingJournalModel** ([OSDevGrp.OSIntranet.WebApi/Models/Accounting/ApplyPostingJournalModel.cs](OSDevGrp.OSIntranet.WebApi/Models/Accounting/ApplyPostingJournalModel.cs)) — carries `AccountingNumber` and `ApplyPostingLineCollectionModel`
- **ApplyPostingLineModel** ([OSDevGrp.OSIntranet.WebApi/Models/Accounting/ApplyPostingLineModel.cs](OSDevGrp.OSIntranet.WebApi/Models/Accounting/ApplyPostingLineModel.cs)) — line data with `Identifier`, `PostingDate`, `Reference`, `AccountNumber`, `Details`, `BudgetAccountNumber`, `Debit`, `Credit`, `ContactAccountNumber`, `SortOrder`

### Command Pattern (Bff.DomainServices)
- **PostingLineFeatureBase<T>** ([OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs)) — abstract base where subclasses implement `ProcessPostingJournalAsync(ApplyPostingJournalModel, TRequest, CancellationToken)`
- **PostingJournalLineDataRequestBase** ([OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineDataRequestBase.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineDataRequestBase.cs)) — contains posting line properties: `PostingDate`, `PostingReference`, `Account`, `PostingText`, `BudgetAccount`, `Debit`, `Credit`, `ContactAccount`
- **PostingJournalLineIdentificationRequestBase** ([OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineIdentificationRequestBase.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineIdentificationRequestBase.cs)) — adds `Identifier` property
- **IStaticTextProvider** — provides localized error messages via `StaticTextKey` enum
- **StaticTextKey.IdentifierAlreadyExists** — message key for duplicate identifier error

### Query Pattern (Bff.DomainServices)
- **PostingJournalRequest** ([OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalRequest.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalRequest.cs)) — query for fetching a journal
- **PostingJournalFeature** — handler that executes the query
- **PostingJournalResponse** — response with updated journal data

### API Layer (Bff.WebApi)
- **PostingJournalResponseDto** ([OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/PostingJournalResponseDto.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/PostingJournalResponseDto.cs)) — maps domain response to JSON with `DynamicTexts`, `StaticTexts`, `ValidationRuleSet`
- **AccountingRuleSetSpecifications** — validation constants: `PostingReferenceMinLength`, `PostingReferenceMaxLength`, `AccountNumberMinLength`, `AccountNumberMaxLength`, `AccountNumberRegexPattern`, `PostingTextMinLength`, `PostingTextMaxLength`, `DebitMinValue`, `DebitMaxValue`, `CreditMinValue`, `CreditMaxValue`
- **AccountingController** ([OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs)) — endpoint host, uses `[FromServices]` to inject `IQueryFeature<T, TResponse>` and command features
- **Policies.AccountingModifier** — authorization policy for modification operations

### Exceptions
- **IdentifierAlreadyExistsException** — thrown when a duplicate identifier is detected; includes identifier and message

## Goal

Implement a complete append-posting-line workflow across the BFF layer:

1. **Command layer** (DomainServices): Define `AppendPostingLineToPostingJournalRequest` and `AppendPostingLineToPostingJournalFeature` that validate uniqueness, create a new posting line, calculate sort order, and return the modified journal.

2. **Base class enhancement**: Add `SortAndClonePostingJournal()` method to `PostingLineFeatureBase` to sort lines by posting date (descending) and sort order (descending within same date), then return a cloned journal.

3. **API DTOs** (WebApi): Create `PostingJournalLineModifierDtoBase` abstract class as a reusable base for posting line modification requests, and `AppendPostingLineToPostingJournalDto` inheriting from it.

4. **API Endpoint** (WebApi): Expose `POST /api/accounting/{accountingNumber}/postingjournal/postinglines` that accepts the DTO, executes the command feature, queries the updated journal via `PostingJournalFeature`, and returns the result as `PostingJournalResponseDto`.

5. **Test coverage**: Comprehensive unit tests for command logic (duplicate detection, line creation, sort order calculation) and WebApi endpoint (request building, feature invocation, response mapping).

## User Stories

- **As a React client**, I want to `POST` a new posting line to an accounting's journal and receive the complete updated journal in the response, so that my UI can display the latest state without a separate fetch.

- **As a domain service**, I want to validate that posting line identifiers are unique within a journal and fail fast with a localized error message if a duplicate is detected, so that data integrity is maintained.

- **As an accountant**, I want posting lines to be sorted by posting date (most recent first), and within the same date by creation order (most recently created first), so that the journal displays in chronological and logical order.

## Acceptance Criteria

1. ✅ `AppendPostingLineToPostingJournalRequest` inherits from `PostingJournalLineDataRequestBase` and passes all parameters to base constructor
2. ✅ `AppendPostingLineToPostingJournalFeature` inherits from `PostingLineFeatureBase<AppendPostingLineToPostingJournalRequest>` and implements `ProcessPostingJournalAsync`
3. ✅ `ProcessPostingJournalAsync` checks for duplicate identifier in the journal's existing lines:
   - If found, throws `IdentifierAlreadyExistsException(identifier, staticTextMessage)`
   - Static text message retrieved from `StaticTextProvider.GetStaticTextAsync(StaticTextKey.IdentifierAlreadyExists, ...)`
4. ✅ `ProcessPostingJournalAsync` creates new `ApplyPostingLineModel` with all properties from request (PostingDate, Reference→Reference, Account→AccountNumber, PostingText→Details, BudgetAccount→BudgetAccountNumber, Debit, Credit, ContactAccount→ContactAccountNumber)
5. ✅ New line's `Identifier` set from `request.Identifier`
6. ✅ New line's `SortOrder` calculated as: `max(existing line SortOrder values) + 1`, or `1` if no lines exist
7. ✅ New line added to collection of existing lines
8. ✅ Collection passed to `SortAndClonePostingJournal(existingJournal, updatedLines)` → returns new cloned `ApplyPostingJournalModel`
9. ✅ `SortAndClonePostingJournal` sorts by `PostingDate` (descending), then `SortOrder` (descending within same date)
10. ✅ Returned model is a different instance (cloned), preserves `AccountingNumber`, and contains sorted lines
11. ✅ `PostingJournalLineModifierDtoBase` is public abstract class in `Controllers/Accounting/Dtos/` with properties:
    - `PostingDate` ([Required], DateTimeOffset)
    - `PostingReference` ([MinLength(PostingReferenceMinLength)] [MaxLength(PostingReferenceMaxLength)], optional string)
    - `Account` ([Required] [MinLength(AccountNumberMinLength)] [MaxLength(AccountNumberMaxLength)] [RegularExpression(AccountNumberRegexPattern)], string)
    - `PostingText` ([Required] [MinLength(PostingTextMinLength)] [MaxLength(PostingTextMaxLength)], string)
    - `BudgetAccount` ([MinLength(AccountNumberMinLength)] [MaxLength(AccountNumberMaxLength)] [RegularExpression(AccountNumberRegexPattern)], optional string)
    - `Debit` ([Range(DebitMinValue, DebitMaxValue)], optional decimal)
    - `Credit` ([Range(CreditMinValue, CreditMaxValue)], optional decimal)
    - `ContactAccount` ([MinLength(AccountNumberMinLength)] [MaxLength(AccountNumberMaxLength)] [RegularExpression(AccountNumberRegexPattern)], optional string)
12. ✅ `AppendPostingLineToPostingJournalDto` inherits from `PostingJournalLineModifierDtoBase`, adds `Identifier` ([Required], Guid)
13. ✅ Endpoint `AppendPostingLineToPostingJournalAsync` created in `AccountingController` with:
    - Route: `[HttpPost("{accountingNumber:int}/postingjournal/postinglines")]`
    - Authorization: `[Authorize(Policy = Policies.AccountingModifier)]`
    - Dependency injection: `ICommandFeature<AppendPostingLineToPostingJournalDto>`, `IQueryFeature<PostingJournalRequest, PostingJournalResponse>`, `int accountingNumber`, `AppendPostingLineToPostingJournalDto dto`, `CancellationToken`
    - ProducesResponseType attributes for 200 (OK), 400 (BadRequest), 401 (Unauthorized), 500 (InternalServerError)
14. ✅ Endpoint retrieves security context via `_securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken)`
15. ✅ Endpoint creates `AppendPostingLineToPostingJournalRequest` from DTO properties, accounting number, and security context
16. ✅ Endpoint executes command feature: `await commandFeature.ExecuteAsync(appendRequest, cancellationToken)`
17. ✅ Endpoint creates `PostingJournalRequest` with accounting number, current date status (via `_timeProvider`), format provider, and security context
18. ✅ Endpoint executes query feature: `await queryFeature.ExecuteAsync(postingJournalRequest, cancellationToken)`
19. ✅ Endpoint maps response: `PostingJournalResponseDto.Map(postingJournalResponse)` and returns `Ok(...)`
20. ✅ DomainServices.Tests: `ExecuteAsyncTests.cs` covers:
    - Duplicate identifier detection and exception throwing
    - New line creation with all properties mapped correctly
    - Sort order calculation (1 for empty, max+1 for existing)
    - Line collection management
    - Cloned journal return with sorting validation
21. ✅ WebApi.Tests: `AppendPostingLineToPostingJournalAsyncTests.cs` covers:
    - Command feature execution verification (mock called exactly once, request properties correct)
    - Query feature execution verification (mock called exactly once, request properties correct)
    - Security context provider invocation
    - Response type assertion (OkObjectResult with PostingJournalResponseDto)
    - Full integration flow

## Scope

### In scope
- `AppendPostingLineToPostingJournalRequest` class in OSDevGrp.OSIntranet.Bff.DomainServices
- `AppendPostingLineToPostingJournalFeature` class in OSDevGrp.OSIntranet.Bff.DomainServices
- `SortAndClonePostingJournal` method in `PostingLineFeatureBase` in OSDevGrp.OSIntranet.Bff.DomainServices
- All unit tests in OSDevGrp.OSIntranet.Bff.DomainServices.Tests for the above
- `PostingJournalLineModifierDtoBase` abstract class in OSDevGrp.OSIntranet.Bff.WebApi
- `AppendPostingLineToPostingJournalDto` class in OSDevGrp.OSIntranet.Bff.WebApi
- `AppendPostingLineToPostingJournalAsync` endpoint in `AccountingController` in OSDevGrp.OSIntranet.Bff.WebApi
- All unit tests in OSDevGrp.OSIntranet.Bff.WebApi.Tests for the endpoint

### Out of scope
- React component implementation (manual testing only per business requirements)
- ServiceGateways modifications (data access contracts unchanged)
- Update and delete posting line features (future iterations)
- Database schema or EF migrations (no structural changes needed)

## Risks

- **Identifier collision in production**: If clients reuse identifiers across requests, the validation will catch it, but callers must handle the 400 BadRequest gracefully. Ensure React client generates unique UUIDs per line.
- **Sort order gaps**: If lines are deleted externally between appending and sorting, max(SortOrder) might skip integers — this is acceptable for ordering but could confuse audit logs if sort order is interpreted as sequential. Document that SortOrder is for display order only.
- **Cloning overhead**: `SortAndClonePostingJournal` clones the entire journal model and all lines on every append. For journals with thousands of lines, this could be expensive. Acceptable for typical use cases; monitor performance if journals grow.
- **Static text provider failures**: If `GetStaticTextAsync` fails or returns null, the exception message will be missing. Ensure StaticTextProvider always returns a default message for `IdentifierAlreadyExists`.
- **TimeProvider resolution**: Endpoint uses `_timeProvider.GetUtcNow()` to determine current date for journal query. If system clock changes during test or request, timing assertions in tests must account for this — use fixture-controlled time in mocks.
