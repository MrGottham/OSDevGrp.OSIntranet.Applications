# PRD: Modify Posting Line in Accounting Posting Journal

## Problem

Currently, the accounting application only allows users to **append** posting lines to a posting journal. If a user needs to correct an existing posting line entry, they must delete it and re-add it from scratch, which is inefficient and poor UX. Users need the ability to **modify** (edit) posting lines they have already created, so they can fix mistakes, update amounts, or adjust account assignments without workflow disruption.

## Relevant Codebase

### Architecture Overview
The BFF WebApi follows a three-layer pattern:
1. **ServiceGateways** (out of scope) — Persistent data layer; provides `GetPostingJournalAsync` and `SavePostingJournalAsync` for posting journal I/O
2. **BFF DomainServices** — Command/Query feature layer; orchestrates business logic, permissions, and static text retrieval
3. **BFF WebApi** — ASP.NET Core endpoint layer; routes HTTP requests to features via dependency injection
4. **React App** — Frontend; calls WebApi endpoints via `AccountingService`

### Existing Append Feature (Pattern to Follow)

**DomainServices layer:**
- [PostingJournalLineDataRequestBase.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineDataRequestBase.cs) — Base request class with 8 properties (PostingDate, PostingReference, Account, PostingText, BudgetAccount, Debit, Credit, ContactAccount)
- [PostingJournalLineIdentificationRequestBase.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineIdentificationRequestBase.cs) — Parent of above; adds AccountingNumber, Identifier, and SecurityContext
- [AppendPostingLineToPostingJournalRequest.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AppendPostingLineToPostingJournal/AppendPostingLineToPostingJournalRequest.cs) — Sealed request inheriting from PostingJournalLineDataRequestBase
- [AppendPostingLineToPostingJournalFeature.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AppendPostingLineToPostingJournal/AppendPostingLineToPostingJournalFeature.cs) — Executes append logic via `ProcessPostingJournalAsync`; inherits from `PostingLineFeatureBase<AppendPostingLineToPostingJournalRequest>`
- [PostingLineFeatureBase.cs](OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs) — Template orchestrates: GetPostingJournal → ProcessPostingJournal → SavePostingJournal
- `SortAndClonePostingJournal` method (in base) — Sorts lines by PostingDate (desc) then SortOrder (desc), clones the journal

**Key data model:**
- `ApplyPostingLineModel` is generated (immutable); lines have `Identifier`, `SortOrder`, `PostingDate`, and 8 data properties
- To modify a line: construct a new `ApplyPostingLineModel` with updated data, preserve its `Identifier` and `SortOrder`, replace in collection

**WebApi layer:**
- [PostingJournalLineModifierDtoBase.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/PostingJournalLineModifierDtoBase.cs) — Abstract DTO base with 8 validation-decorated properties (matches request fields)
- [AppendPostingLineToPostingJournalDto.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/AppendPostingLineToPostingJournalDto.cs) — Concrete DTO adding `Identifier` property
- [AccountingController.cs](OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs#L184) — `AppendPostingLineToPostingJournalAsync` endpoint:
  - Route: POST `/api/accounting/{accountingNumber}/postingjournal/postinglines`
  - Authorization: `[Authorize(Policy = Policies.AccountingModifier)]`
  - Accepts: `ICommandFeature<AppendPostingLineToPostingJournalRequest>` and `IQueryFeature<PostingJournalRequest, PostingJournalResponse>`
  - Flow: DTO → Request → CommandFeature.ExecuteAsync → QueryFeature.ExecuteAsync (refresh) → PostingJournalResponseDto.Map

**Testing conventions (WebApi):**
- [AppendPostingLineToPostingJournalAsyncTests.cs](OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/AppendPostingLineToPostingJournalAsyncTests.cs) — 16 test methods:
  - Command feature invocation (request fields, security context)
  - Query feature invocation (request ID, accounting number, status date, format provider)
  - Response types (OK, BadRequest, Unauthorized, InternalServerError)
  - AutoFixture + Moq for setup; categorized `[Category("UnitTest")]`

**React layer:**
- [AccountingService.jsx](osdevgrp.osintranet.react/src/services/AccountingService.jsx#L119) — `appendPostingLineToPostingLineJournal` method validates required fields, builds JSON body, sends POST with antiforgery header, returns response.json()
- [PostingJournal.jsx](osdevgrp.osintranet.react/src/components/PostingJournal.jsx#L451) — `handleModifyPostingJournalLine` opens edit modal; `handleUpdatePostingJournalLine` is currently a stub (console.debug only)

### Integration Points

**Security flow:**
- Endpoint calls `_securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken)`
- Request carries `SecurityContext` to feature
- Feature verifies permissions (implicit in base class) with policy `AccountingModifier`

**Response flow:**
- Command modifies journal in gateway
- Query fetches fresh journal state (for consistency)
- Returns `PostingJournalResponseDto.Map(postingJournalResponse)`
- React updates component state from `response.dynamicTexts.postingJournal`

**Error handling:**
- Missing identifier: throw `UnknownIdentifierException` with request identifier and localized text from `StaticTextKey.UnknownIdentifier`
- Invalid DTO fields: validation attributes on DTO (AccountingRuleSetSpecifications min/max/regex)
- Authorization failure: implicit in endpoint `[Authorize]` attribute

### Patterns to Follow

1. **Request class**: Inherit from `PostingJournalLineDataRequestBase`; no additional properties needed
2. **Feature class**: Inherit from `PostingLineFeatureBase<YourRequest>`; implement `ProcessPostingJournalAsync` to find/replace line and return `SortAndClonePostingJournal`
3. **DTO class**: Inherit from `PostingJournalLineModifierDtoBase`; add route-bound properties (e.g., Identifier) with `[Required]`
4. **Endpoint method**: Accept command and query features as `[FromServices]`, build request from route + DTO + SecurityContext, execute both, return OK with response DTO
5. **Tests**: Use AutoFixture and Moq; verify each collaborator is called with correct arguments; group by feature/query/response concerns

## Goal

Enable accounting users to modify posting lines they have added to a posting journal. When complete:
- Users can click an edit icon on a posting line, modify fields in a modal form, and save the changes
- The API receives the modified data, updates the line while preserving its identity and position in the journal, and returns the refreshed posting journal
- The UI closes the edit modal, updates the displayed journal, and confirms the change to the user

## User Stories

**As an accounting user**, I want to modify a posting line I have already created in the posting journal **so that** I can correct mistakes, update amounts, or adjust account assignments without deleting and re-adding the entry.

## Acceptance Criteria

### Iteration 1: DomainServices Command Logic

1. `ModifyPostingLineInPostingJournalRequest.cs` exists in namespace `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting`, inherits from `PostingJournalLineDataRequestBase`, and is sealed.
2. `ModifyPostingLineInPostingJournalFeature.cs` exists in the same namespace, inherits from `PostingLineFeatureBase<ModifyPostingLineInPostingJournalRequest>`, and is internal sealed.
3. Feature's `ProcessPostingJournalAsync` finds the posting line matching `request.Identifier` in `postingJournal.ApplyPostingLines`.
4. If no line matches the identifier, the feature throws `UnknownIdentifierException` with the request identifier and retrieves the error message from `StaticTextKey.UnknownIdentifier`.
5. If a line is found, feature constructs a replacement `ApplyPostingLineModel` with all request data fields, **preserving** the matched line's `Identifier` and `SortOrder`.
6. Feature replaces only the matched line in the collection, leaves all other lines unchanged, and returns the cloned journal via `SortAndClonePostingJournal`.
7. Unit tests in `ModifyPostingLineInPostingJournalFeature/ExecuteAsyncTests.cs` verify:
   - A matching line is replaced with all request fields while retaining identifier and SortOrder
   - Missing identifier throws `UnknownIdentifierException`, includes the requested identifier, retrieves message from `StaticTextKey.UnknownIdentifier`
   - Other lines remain unchanged and journal is sorted by posting date and sort order
   - `SavePostingJournalAsync` is called with the updated journal, accounting number, and cancellation token
8. All existing DomainServices tests pass (680+ tests); build is clean.

### Iteration 2: WebApi Endpoint & DTO

1. `ModifyPostingLineInPostingJournalDto.cs` exists in namespace `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`, inherits from `PostingJournalLineModifierDtoBase`, and contains no additional properties.
2. `ModifyPostingLineInPostingJournalAsync` method is added to `AccountingController`:
   - Route: `[HttpPut("{accountingNumber:int}/postingjournal/postinglines/{identifier}")]`
   - Authorization: `[Authorize(Policy = Policies.AccountingModifier)]`
   - Produces response types documented: OK (PostingJournalResponseDto), BadRequest, Unauthorized, InternalServerError (all with ProblemDetails)
   - Accepts `[FromServices] ICommandFeature<ModifyPostingLineInPostingJournalRequest> commandFeature` and `[FromServices] IQueryFeature<PostingJournalRequest, PostingJournalResponse> queryFeature`
   - Binds route parameters: `accountingNumber` (int, validated), `identifier` (required GUID)
   - Binds DTO from `[FromBody]` and accepts `CancellationToken`
   - Resolves security context, creates `ModifyPostingLineInPostingJournalRequest` with all values and context
   - Executes command feature, then query feature with current status date, format provider, and security context
   - Returns `Ok(PostingJournalResponseDto.Map(postingJournalResponse))`
3. Unit tests in `ModifyPostingLineInPostingJournalAsyncTests.cs` (same namespace pattern as Append) verify:
   - `GetCurrentSecurityContextAsync` is called once with given cancellation token
   - Command feature is called exactly once with `ModifyPostingLineInPostingJournalRequest` whose properties match route/DTO/context
   - Query feature is called once with correct accounting number, status date, format provider, security context, and cancellation token
   - Method returns `OkObjectResult` containing `PostingJournalResponseDto`
   - Response types are correct (status codes, content types)
4. All WebApi tests pass (680+ tests); build is clean.

### Iteration 3: React Service & Component Integration

1. `AccountingService.jsx` includes method `modifyPostingLineInPostingLineJournal(accountingNumber, identifier, isoPostingDateString, postingReference, account, postingText, budgetAccount, debit, credit, contactAccount)`:
   - Validates required parameters (accountingNumber, identifier, isoPostingDateString, account, postingText)
   - Builds JSON body with all fields (omits null/undefined optional fields like Append does)
   - Sends PUT request to `/api/accounting/{accountingNumber}/postingjournal/postinglines/{identifier}`
   - Includes antiforgery header and credentials; follows Append method's error handling pattern
   - Returns `response.json()` on success; throws error on failure
2. `PostingJournal.jsx` `handleUpdatePostingJournalLine` implementation:
   - Accepts same parameters as `handleModifyPostingJournalLine`
   - Calls `accountingService.modifyPostingLineInPostingLineJournal` with field conversions: integer accounting number, UUID identifier, ISO posting date, nullable strings for optional fields, strings for required fields, nullable decimals for amounts
   - On success: closes edit modal, updates journal from `response.dynamicTexts`, clears form state
   - On failure: shows danger toast with error message, keeps modal open for retry
   - Removes any temporary `console.debug` calls from stub implementation
3. Manual testing confirms:
   - Edit form populates correctly with existing line data
   - Modifying and saving updates the displayed line
   - Refreshed journal shows modified line with correct values and correct sort position
   - Canceling edit modal does not submit
   - Error messages display on validation or server failure

## Scope

### In Scope
- **DomainServices**: Command request and feature to replace posting line, preserving identifier and sort order
- **WebApi**: PUT endpoint, DTO, authorization, response handling
- **React**: Service method and component integration
- **Testing**: Unit tests for DomainServices feature and WebApi endpoint (per project conventions); manual validation for React UI
- **Error handling**: `UnknownIdentifierException` for missing line; DTO validation for bad data
- **Authorization**: AccountingModifier policy (write-level access)

### Out of Scope
- **ServiceGateways**: Posting journal persistence already exists (no changes to data layer)
- **Database/EF Core**: No migrations or schema changes
- **React automated tests**: Manual UI validation only (per project convention for React components)
- **Deleting posting lines**: Different feature; left for future work
- **Bulk modifications**: Applies to single line only
- **Audit logging**: If required, handled by existing gateway/persistence layer

## Risks

1. **Immutable model construction**: `ApplyPostingLineModel` is generated with immutable properties. Must construct replacement with **all** fields from request, preserving only `Identifier` and `SortOrder`. Missing one field or changing either preserved property will corrupt the journal.
   - *Mitigation*: Review Append feature's constructor call closely; copy the pattern exactly for Modify; unit test verifies line is cloned correctly.

2. **Sort order integrity**: Lines must be re-sorted by PostingDate (desc), then SortOrder (desc) after replacement. If `SortAndClonePostingJournal` is not called or sorting logic changes, journal display will be wrong.
   - *Mitigation*: Feature must always call `SortAndClonePostingJournal` with updated collection; test verifies sorting is applied.

3. **Authorization policy mismatch**: Endpoint uses `[Authorize(Policy = Policies.AccountingModifier)]` (write access). If wrong policy is applied, users may get 403 Forbidden or unintended access.
   - *Mitigation*: Verify policy name matches the Append endpoint exactly; test confirms security context is resolved correctly.

4. **Route identifier binding**: Identifier is a GUID in the route path. If binding fails or is not validated, request may proceed with invalid/null identifier, causing logic errors.
   - *Mitigation*: Add `[Required]` attribute to route identifier; test verifies identifier is passed correctly to feature.

5. **Modal state confusion**: React component must distinguish between "create new line" and "modify existing line" modes. If form state is not properly reset between modes, modifications may affect wrong line.
   - *Mitigation*: Component already has separate handlers (`handleCreatePostingJournalLine` vs `handleUpdatePostingJournalLine`); verify modal state is cleared on close.

6. **Null/undefined field handling**: Optional fields (PostingReference, BudgetAccount, etc.) can be null/undefined. JSON serialization and DTO binding must handle omitted vs. explicit null correctly.
   - *Mitigation*: Follow Append service method's pattern (omit null fields from JSON body); DTO allows nullable properties; test covers various null/present scenarios.

---

**Task Directory**: `tasks/2026-10-07-modify-posting-line/`  
**Task Date**: 2026-10-07  
**Task Slug**: modify-posting-line
