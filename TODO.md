# Appending posting journal for an accounting

## General

We need to append/implement functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.DomainServices (new Query Feature layer)
* OSDevGrp.OSIntranet.Bff.WebApi (new endpoint + DTO)
* **ALREADY IMPLEMENTED:** `IAccountingGateway.GetPostingJournalAsync()` exists in ServiceGateways.Interfaces and AccountingGateway implementation
* **ALREADY IMPLEMENTED:** Test data builders (`CreateApplyPostingJournalModel()`) exist in ServiceGateways.TestData
* **OUT OF SCOPE:** osdevgrp.osintranet.react

We need to create tests for functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.DomainServices.Tests (ExecuteAsyncTests, VerifyPermissionAsyncTests for PostingJournalFeature)
* OSDevGrp.OSIntranet.Bff.WebApi.Tests (PostingJournalAsyncTests for controller endpoint)

**Test Fixtures/Helpers Already Available:**
* `CreateApplyPostingJournalModel()` in OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData.FixtureExtensions
* `CreatePostingJournalTexts()` in OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos.FixtureExtensions
* `CreateStaticTexts()`, `CreateValidationRuleSet()` in OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos.FixtureExtensions

**OUT OF SCOPE:** Note: No automated tests are needed for osdevgrp.osintranet.react as the React component will be validated through manual testing.

## Expose an accountings given posting journal from the BFF WebApi

### Business Gool

The WebApi should be able to expse the posting journal for a given accounting. This should support the React application to reload the posting journal when then React application add, updates or delete lines in the posting journal.

### Need functinoalitety in the domain service layer

**No new interfaces needed** - All required interfaces already exist:
* `IPostingJournalTexts` - Existing interface for posting journal dynamic text resources
* `IPostingJournalTextsBuilder` - Existing builder already registered in ServiceCollectionExtensions
* `IPostingJournalRuleSetBuilder` - Existing builder already registered in ServiceCollectionExtensions
* `IAccountingGateway.GetPostingJournalAsync()` - Already implemented in ServiceGateways

**Implementation required:**

* Create `PostingJournalRequest` class in `Features/Queries/Accounting/PostingJournal/` inheriting from `AccountingIdentificationRequestBase`
* Create `PostingJournalResponse` class in `Features/Queries/Accounting/PostingJournal/` inheriting from `AccountingIdentificationResponseBase<ApplyPostingJournalModel, IPostingJournalTexts>` (reusing existing `ApplyPostingJournalModel` from WebApi.ClientApi and existing `IPostingJournalTexts`)
* Create `PostingJournalFeature` class in `Features/Queries/Accounting/PostingJournal/` inheriting from `AccountingIdentificationFeatureBase<PostingJournalRequest, PostingJournalResponse, ApplyPostingJournalModel, IPostingJournalTexts, IPostingJournalTextsBuilder, IPostingJournalRuleSetBuilder>`
  * Implement `GetModelAsync()` to call `IAccountingGateway.GetPostingJournalAsync(request.AccountingNumber, cancellationToken)`
  * Implement `BuildResponseAsync()` to return new `PostingJournalResponse(model, postingJournalTexts, staticTexts, validationRuleSet)`
  * Implement `GetStaticTextSpecifications()` to include all 25 static text keys: `PostingJournal`, `PostingDate`, `PostingReference`, `Account`, `AccountName`, `PostingText`, `BudgetAccount`, `Posted`, `Available`, `Debit`, `Credit`, `ContactAccount`, `Balance`, `PostingValue`, `AddPostingJournalLine`, `UpdatePostingJournalLine`, `DeletePostingJournalLine`, `PostingJournalLineDeletionQuestion`, `Create`, `Update`, `Delete`, `ConfirmDeletion`, `DeleteVerificationInfo`, `Reset`, `Cancel`
  * Override `VerifyPermissionAsync()` to require user be authenticated, have accounting access, and be an accounting modifier (not just viewer) for the specific accounting number using `IPermissionChecker.IsAuthenticated()`, `IPermissionChecker.HasAccountingAccess()`, and `IPermissionChecker.IsAccountingModifier(user, accountingNumber)`
* Reuse existing `PostingJournalTextsBuilder` implementation in `OSDevGrp.OSIntranet.Bff.DomainServices/Logic/DynamicText/` (already implements `IPostingJournalTextsBuilder` and is registered in `ServiceCollectionExtensions`)
* Reuse existing `PostingJournalRuleSetBuilder` implementation in `OSDevGrp.OSIntranet.Bff.DomainServices/Logic/Validation/` (already implements `IPostingJournalRuleSetBuilder` and is registered in `ServiceCollectionExtensions`)
* `PostingJournalFeature` will be auto-discovered and registered by the `AddFeatures()` call in `ServiceCollectionExtensions.AddDomainServices()`

### Test strategy for the domain service layer

Create `ExecuteAsyncTests` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/PostingJournal/PostingJournalFeature/ExecuteAsyncTests.cs`:

* Test that `GetPostingJournalAsync` is called on `IAccountingGateway` with correct `AccountingNumber` from request
* Test that `GetPostingJournalAsync` is called with correct `CancellationToken`
* Test that response contains `PostingJournal` equal to model resolved by gateway
* Test that response contains `DynamicTexts` equal to `PostingJournalTexts` resolved by `PostingJournalTextsBuilder`
* Test that response contains `ValidationRuleSet` equal to validation rules resolved by `PostingJournalRuleSetBuilder`
* Test all 25 `StaticTextKey` values are requested in `GetStaticTextSpecifications()`:
  * Field headers: `PostingJournal`, `PostingDate`, `PostingReference`, `Account`, `PostingText`, `BudgetAccount`, `Debit`, `Credit`, `ContactAccount`
  * Field labels: `AccountName`, `Posted`, `Available`, `Balance`, `PostingValue`
  * Action texts: `AddPostingJournalLine`, `UpdatePostingJournalLine`, `DeletePostingJournalLine`, `PostingJournalLineDeletionQuestion`
  * Dialog/button texts: `Create`, `Update`, `Delete`, `ConfirmDeletion`, `DeleteVerificationInfo`, `Reset`, `Cancel`

Create `VerifyPermissionAsyncTests` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/PostingJournal/PostingJournalFeature/VerifyPermissionAsyncTests.cs`:

* Test that `User` is accessed from the given `ISecurityContext`
* Test that `IsAuthenticated` is called on `IPermissionChecker` with user from `ISecurityContext`
* Test that when user is authenticated, `HasAccountingAccess` is called on `IPermissionChecker` with user from `ISecurityContext`
* Test that when user is authenticated and has accounting access, `IsAccountingModifier` is called on `IPermissionChecker` with user from `ISecurityContext` and `AccountingNumber` from request (note: uses **modifier** not **viewer** unlike base implementation)
* Test `VerifyPermissionAsync` returns `false` when user is not authenticated
* Test `VerifyPermissionAsync` returns `false` when user is authenticated but doesn't have accounting access
* Test `VerifyPermissionAsync` returns `false` when user is authenticated and has accounting access but is not an accounting modifier
* Test `VerifyPermissionAsync` returns `true` when user is authenticated and has accounting access and is an accounting modifier

### Need functinoalitety in the BFF WebApi layer

* Add `AccountingModifier` policy constant to `OSDevGrp.OSIntranet.Bff.WebApi/Security/Policies.cs`
  * Add constant: `internal const string AccountingModifier = "AccountingModifier";`
* Add `AccountingModifier` authorization policy to `OSDevGrp.OSIntranet.Bff.WebApi/Program.cs`
  * Add after `AccountingViewer` policy:
    ```csharp
    options.AddPolicy(Policies.AccountingModifier, policy =>
    {
        policy.AddAuthenticationSchemes(Schemes.Internal);
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireClaim(ClaimTypes.Name);
        policy.RequireClaim(ClaimTypes.Email);
        policy.RequireClaim(OSDevGrp.OSIntranet.Bff.DomainServices.Security.ClaimTypes.AccountingClaimType);
        policy.RequireClaim(OSDevGrp.OSIntranet.Bff.DomainServices.Security.ClaimTypes.AccountingModifierClaimType);
    });
    ```
* Create `PostingJournalResponseDto` class in `Controllers/Accounting/Dtos/` inheriting from `AccountingIdentificationDto`
  * Add properties (no extra properties needed, only these three): 
    - `PostingJournalTextsDto DynamicTexts` [Required]
    - `IReadOnlyCollection<StaticTextDto> StaticTexts` [Required]
    - `ValidationRuleSetDto ValidationRuleSet` [Required]
  * Inherits `Number` property from base `AccountingIdentificationDto`
  * Implement `Map()` method: `internal static PostingJournalResponseDto Map(PostingJournalResponse postingJournalResponse)` that populates all properties from response object
* Create GET endpoint `/api/accounting/{accountingNumber}/postingjournal` to retrieve posting journal
  * Add using statement: `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;`
  * Add method to `AccountingController`:
    ```csharp
    [Authorize(Policy = Policies.AccountingModifier)]
    [HttpGet("{accountingNumber:int}/postingjournal")]
    [ProducesResponseType(typeof(PostingJournalResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> PostingJournalAsync([FromServices] IQueryFeature<PostingJournalRequest, PostingJournalResponse> queryFeature, [FromRoute][Required][Range(AccountingRuleSetSpecifications.AccountingNumberMinValue, AccountingRuleSetSpecifications.AccountingNumberMaxValue)] int accountingNumber, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        PostingJournalRequest postingJournalRequest = new PostingJournalRequest(Guid.NewGuid(), accountingNumber, ResolveStatusDate(null), _formatProvider, securityContext);
        PostingJournalResponse postingJournalResponse = await queryFeature.ExecuteAsync(postingJournalRequest, cancellationToken);

        return Ok(PostingJournalResponseDto.Map(postingJournalResponse));
    }
    ```
* Configure NSwag code generation for the new endpoint
* Update `WebApiClient.generated.cs` after endpoint creation

### Test strategy for the BFF WebApi layer

Create `PostingJournalAsyncTests` in `OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/PostingJournalAsyncTests.cs` following the same pattern as `AccountingAsyncTests`:

* Test that `GetCurrentSecurityContextAsync` is called on `ISecurityContextProvider` with given `CancellationToken`
* Test that `ExecuteAsync` is called on `IQueryFeature<PostingJournalRequest, PostingJournalResponse>` with `PostingJournalRequest` where `RequestId` is not equal to `Guid.Empty`
* Test that `ExecuteAsync` is called with correct `AccountingNumber` from route parameter
* Test that `ExecuteAsync` is called with `StatusDate` equal to today's date (resolved via `ResolveStatusDate(null)`)
* Test that `ExecuteAsync` is called with correct `FormatProvider` from dependencies
* Test that `ExecuteAsync` is called with correct `SecurityContext` from `ISecurityContextProvider`
* Test that `ExecuteAsync` is called with given `CancellationToken`
* Test that method returns `OkObjectResult`
* Test that `OkObjectResult.Value` is of type `PostingJournalResponseDto`

## Acceptance Criteria

Feature is complete when ALL of the following are true:

### Code Quality & Architecture
- [ ] All classes follow established architectural patterns (AccountingIdentificationFeatureBase, AccountingIdentificationResponseBase, etc.)
- [ ] No new interfaces created - only reusing existing ones (IPostingJournalTexts, IPostingJournalTextsBuilder, IPostingJournalRuleSetBuilder)
- [ ] Code follows project naming conventions and namespace structure
- [ ] All classes include proper #region blocks (Constructor, Properties, Methods, Nested classes)
- [ ] Null guards applied using NullGuard.NotNull() where appropriate
- [ ] Proper exception handling with IntranetExceptionBuilder

### DomainServices Implementation
- [ ] `PostingJournalRequest` created and inherits from `AccountingIdentificationRequestBase`
- [ ] `PostingJournalResponse` created and inherits from `AccountingIdentificationResponseBase<ApplyPostingJournalModel, IPostingJournalTexts>`
- [ ] `PostingJournalFeature` created with:
  - [ ] Inherits from `AccountingIdentificationFeatureBase<PostingJournalRequest, PostingJournalResponse, ApplyPostingJournalModel, IPostingJournalTexts, IPostingJournalTextsBuilder, IPostingJournalRuleSetBuilder>`
  - [ ] `GetModelAsync()` calls `IAccountingGateway.GetPostingJournalAsync()` with correct parameters
  - [ ] `BuildResponseAsync()` returns new `PostingJournalResponse` instance correctly
  - [ ] `GetStaticTextSpecifications()` returns all 25 static text keys
  - [ ] `VerifyPermissionAsync()` override checks: `IsAuthenticated` && `HasAccountingAccess` && `IsAccountingModifier` (NOT viewer)
  - [ ] Class is marked `internal`
  - [ ] Auto-discovered and registered via `AddFeatures()`

### DomainServices Tests
- [ ] `ExecuteAsyncTests` class created with 6 test methods covering:
  - [ ] Gateway method called with correct AccountingNumber
  - [ ] Gateway method called with correct CancellationToken
  - [ ] Response PostingJournal equals gateway model
  - [ ] Response DynamicTexts equals builder output
  - [ ] Response ValidationRuleSet equals builder output
  - [ ] All 25 static text keys requested
- [ ] `VerifyPermissionAsyncTests` class created with 8 test methods covering:
  - [ ] User accessed from SecurityContext
  - [ ] IsAuthenticated called on PermissionChecker
  - [ ] HasAccountingAccess called on PermissionChecker
  - [ ] IsAccountingModifier called on PermissionChecker (KEY: modifier, not viewer)
  - [ ] Returns false when not authenticated
  - [ ] Returns false when no accounting access
  - [ ] Returns false when not accounting modifier
  - [ ] Returns true when all permissions granted
- [ ] All tests marked `[Category("UnitTest")]`
- [ ] All tests use NUnit assertions and Moq mocks

### WebApi Layer
- [ ] `AccountingModifier` constant added to `Policies.cs`
- [ ] `AccountingModifier` authorization policy added to `Program.cs` with:
  - [ ] Required claims: NameIdentifier, Name, Email, AccountingClaimType, AccountingModifierClaimType
  - [ ] Correct scheme: Schemes.Internal
- [ ] `PostingJournalResponseDto` created inheriting from `AccountingIdentificationDto` with:
  - [ ] `PostingJournalTextsDto DynamicTexts` property
  - [ ] `IReadOnlyCollection<StaticTextDto> StaticTexts` property
  - [ ] `ValidationRuleSetDto ValidationRuleSet` property
  - [ ] `Map()` method implemented correctly
  - [ ] Inherits `Number` from base class
- [ ] `PostingJournalAsync` endpoint added to `AccountingController` with:
  - [ ] `[Authorize(Policy = Policies.AccountingModifier)]` attribute
  - [ ] `[HttpGet("{accountingNumber:int}/postingjournal")]` route
  - [ ] All ProducesResponseType attributes
  - [ ] Correct parameters: IQueryFeature, accountingNumber, cancellationToken
  - [ ] SecurityContext resolved via provider
  - [ ] Request created with today's date via `ResolveStatusDate(null)`
  - [ ] Response mapped via `PostingJournalResponseDto.Map()`
  - [ ] Returns `OkObjectResult`
- [ ] Using statement added for `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal`

### WebApi Tests
- [ ] `PostingJournalAsyncTests` class created in correct location with 9 test methods covering:
  - [ ] SecurityContextProvider.GetCurrentSecurityContextAsync called with correct CancellationToken
  - [ ] QueryFeature.ExecuteAsync called with PostingJournalRequest where RequestId != Guid.Empty
  - [ ] ExecuteAsync called with correct AccountingNumber from route
  - [ ] ExecuteAsync called with StatusDate = today's date
  - [ ] ExecuteAsync called with correct FormatProvider
  - [ ] ExecuteAsync called with correct SecurityContext
  - [ ] ExecuteAsync called with correct CancellationToken
  - [ ] Method returns OkObjectResult
  - [ ] OkObjectResult.Value is PostingJournalResponseDto
- [ ] All tests follow AccountingAsyncTests pattern
- [ ] All tests marked `[Category("UnitTest")]`
- [ ] Test fixtures use existing helpers: CreateApplyPostingJournalModel, CreatePostingJournalTexts, CreateStaticTexts, CreateValidationRuleSet

### Build & Compilation
- [ ] Solution builds without errors: `dotnet build OSDevGrp.OSIntranet.Applications.sln`
- [ ] Solution builds without warnings
- [ ] All tests pass: `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"`

### Code Generation & Documentation
- [ ] NSwag post-build regenerates WebApiClient.generated.cs
- [ ] New endpoint appears in OpenAPI documentation at `/openapi/v1.json`
- [ ] Implementation diary updated with completion notes at `docs/diary/2026-08-14-posting-journal-feature.md`
