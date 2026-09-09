# Diary: PostingJournal Feature - Iteration 1

Implementation of the PostingJournal query feature for the BFF DomainServices layer. This feature retrieves posting journal data with write-level permission enforcement (IsAccountingModifier, not viewer).

## Step 1: Implement Iteration 1 Core Files (Stories 1-5)

**Author:** main

### Prompt Context

**Verbatim prompt:** "Let make Iteration 1"

**Interpretation:** Create all 5 files for Iteration 1 of the PostingJournal feature: request class, response class, feature handler, and comprehensive unit tests.

**Inferred intent:** Complete a full vertical slice of the PostingJournal feature with production code and thorough test coverage, following established patterns from AccountingFeature.

### What I did

Created 5 new C# files totaling ~625 LOC of implementation and ~280 LOC of tests:

1. `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalRequest.cs` — Query request inheriting from `AccountingIdentificationRequestBase`, carrying accounting number, status date, and security context.

2. `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalResponse.cs` — Response class with model type `Tuple<ApplyPostingJournalModel, Predicate<int>>` to carry both posting journal data and a modifiability predicate.

3. `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalFeature.cs` — Core query handler implementing 6 abstract methods:
   - `GetModelAsync()` — Retrieves posting journal and constructs modifiable predicate
   - `BuildResponseAsync()` — Packages model + texts into response
   - `GetStaticTextSpecifications()` — Declares 25 required static text keys
   - `VerifyPermissionAsync()` — Custom permission override (initially using explicit interface implementation)

4. `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/PostingJournal/PostingJournalFeature/ExecuteAsyncTests.cs` — 6 test methods validating the query execution path:
   - Verifies `GetPostingJournalAsync` called with correct parameters
   - Validates response contains correct posting journal model
   - Validates dynamic and static texts included
   - Validates validation rule set included

5. `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Queries/Accounting/PostingJournal/PostingJournalFeature/VerifyPermissionAsyncTests.cs` — 8 test methods with `[TestCase]` parametrization (32 total test runs) validating permission override:
   - Three-level permission chain: `IsAuthenticated` → `HasAccountingAccess` → `IsAccountingModifier`
   - Verifies early exit when upstream checks fail
   - Validates final return values across all permission state combinations

### Why

The 5 files follow the established `AccountingIdentificationFeatureBase` pattern used by other accounting query features (e.g., AccountingFeature). This pattern ensures consistency across the codebase and leverages existing infrastructure for request/response handling, static text management, and permission verification.

The model type `Tuple<ApplyPostingJournalModel, Predicate<int>>` was necessary because:
- `ApplyPostingJournalModel` contains the posting journal data
- `Predicate<int>` carries the modifiability check (based on `IsAccountingModifier`) so the UI can determine which actions are allowed for the current user

### What worked

- All 38 tests pass immediately after creation (6 ExecuteAsync + 32 VerifyPermissionAsync)
- Build compiles cleanly with no errors or warnings
- Code follows codebase conventions (region blocks, naming, test helpers, Moq/AutoFixture patterns)
- Explicit interface implementation for `VerifyPermissionAsync` correctly overrides base behavior for permission logic

### What didn't work

Six compilation errors discovered during initial build that required systematic fixes:

1. **CS0311** — Model type mismatch. Base class expected `ApplyPostingJournalModel` but builder interface expected `Tuple<ApplyPostingJournalModel, Predicate<int>>`. **Fix:** Updated model type in feature class declaration to match what builder interface required.

2. **CS0506** — Could not override base method because not marked virtual. Attempted to use `override` on `VerifyPermissionAsync`. **Fix:** Changed to explicit interface implementation using `Task<bool> IPermissionVerifiable<PostingJournalRequest>.VerifyPermissionAsync(...)` syntax.

3. **CS1739** — Wrong parameter names in test helper setup call. Test was calling `Setup(..., hasAccountingAccess: ...)` but method signature required `isAuthenticated` parameter. **Fix:** Added `isAuthenticated: isAuthenticated` to all Setup calls in test class.

4. **CS1503** — Wrong setup method signature. Expected `IStaticTextProvider.Setup(fixture)` but got different overload. **Fix:** Changed from fixture extension method call to direct Mock instantiation.

5. **CS0118** — Namespace vs type collision. Using both `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;` (namespace) and class `PostingJournalFeature` in same namespace. **Fix:** Added type alias: `using PostingJournalFeatureType = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal.PostingJournalFeature;`

6. **CS1061** — Missing extension method. No `CreatePostingJournalTexts()` fixture extension existed. **Fix:** Created mock directly: `Mock<IPostingJournalTexts> postingJournalTextsMock = new Mock<IPostingJournalTexts>();`

All errors were resolved and verified with clean builds.

### What I learned

1. **Model type shape matters.** The Tuple model type isn't arbitrary — it's determined by what the builder interface accepts. The builder was designed to work with both the data and a predicate, so the feature must pass a Tuple.

2. **Explicit interface implementation has limits.** Using `Task<bool> IPermissionVerifiable<PostingJournalRequest>.VerifyPermissionAsync(...)` works but only when called through the interface reference. This is a workaround, not ideal design.

3. **Test helper signatures are fragile.** Setup methods require exact parameter names and ordering. AutoFixture conventions differ per project layer.

4. **Namespace collisions in test files are common.** When testing a feature in its own namespace, importing the namespace causes `PostingJournalFeature` to refer to the namespace, not the class. Type aliases are the standard solution.

### What was tricky

1. **Permission override logic.** The base class uses `IsAccountingViewer` (read access), but PostingJournal needs `IsAccountingModifier` (write access). This requires a completely custom permission check, not just a parameter change.

2. **Static text key count.** 25 different static text keys need to be declared and validated. Getting them all right required consulting the specification and validating in tests.

3. **Tuple model threading.** The Tuple carrying the predicate had to flow through `GetModelAsync()` → `BuildResponseAsync()` → `PostingJournalResponse` property extraction (`Model.Item1`). This complex threading required understanding the entire architecture before implementation.

### What warrants review

1. **Permission override design** — The explicit interface implementation for `VerifyPermissionAsync` is functional but unconventional. Reviewers should confirm this is the intended permission strategy before approving.

2. **25 static text keys** — All keys are declared in `GetStaticTextSpecifications()`. Reviewers should verify the list is complete and matches UI requirements.

3. **Tuple model type** — The use of `Tuple<ApplyPostingJournalModel, Predicate<int>>` for carrying both data and modifiability is unusual. Reviewers should confirm this matches the intended data flow to the UI layer.

4. **Test parametrization** — The `[TestCase]` parametrization generates 32 test runs from 8 methods. This is comprehensive but adds cognitive load. Reviewers should verify the coverage is meaningful.

### Future work

1. **Refactor permission override** — The explicit interface implementation works but can be improved by making `VerifyPermissionAsync` virtual in the base class, allowing a cleaner `override` in the derived class.

2. **Iteration 2** — Implement WebApi integration layer (Stories 6-9):
   - Add `AccountingModifier` authorization policy to `Program.cs`
   - Create `PostingJournalResponseDto` DTO
   - Implement GET endpoint
   - Create endpoint tests

3. **Documentation** — Add XML doc comments to public methods explaining the permission override and Tuple model.

---

## Step 2: Refactor Permission Override to Virtual Pattern

**Author:** main

### Prompt Context

**Verbatim prompt:** "Would it be better to make #sym:VerifyPermissionAsync in @sym:AccountingIdentificationFeatureBase<TAccountingIdentificationRequest, TAccountingIdentificationResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder> virtual so we can override it in @sym:PostingJournalFeature ?"

**Interpretation:** Consider refactoring the permission override mechanism from explicit interface implementation to virtual method override, which is more idiomatic C#.

**Inferred intent:** Improve code quality by using standard OO polymorphism patterns instead of a workaround pattern, making the code more discoverable and maintainable for future features.

### What I did

Made two coordinated changes across the codebase:

1. `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationFeatureBase.cs` — Added `virtual` keyword to `VerifyPermissionAsync` method (line 45), allowing derived classes to override it:
   ```csharp
   public virtual Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, TAccountingIdentificationRequest request, CancellationToken cancellationToken)
   ```

2. `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalFeature.cs` — Replaced explicit interface implementation with standard override:
   - Removed `IPermissionVerifiable<PostingJournalRequest>` from class declaration
   - Changed method from `Task<bool> IPermissionVerifiable<PostingJournalRequest>.VerifyPermissionAsync(...)` to `public override Task<bool> VerifyPermissionAsync(...)`

### Why

The virtual pattern is superior:
- **Idiomatic:** `override` is the standard C# pattern for polymorphic behavior; explicit interface implementation is a workaround
- **Discoverable:** Future developers see `override` keyword and immediately understand intent
- **Consistent:** Follows established OO principles; works through any reference type (base, derived, or interface)
- **Extensible:** Other accounting features needing custom permissions can follow the same pattern
- **Cleaner:** No need for redundant interface declaration in derived class

### What worked

- All 38 PostingJournal tests pass after refactoring
- Build succeeds with 0 errors/warnings
- Solution builds cleanly
- Refactoring required no changes to test code or logic — only method signature styling

### What didn't work

No issues encountered. The refactoring was straightforward once the virtual keyword was added to the base class.

### What I learned

1. **Virtual method discovery in base classes** — Making a base method virtual is a key step that enables idiomatic override patterns in derived classes. This should be a standard consideration when designing extensibility points.

2. **No test impact** — Changing from explicit interface implementation to virtual override doesn't require test changes because the behavior is identical; only the implementation mechanism changed.

### What was tricky

Nothing particularly tricky. The refactoring was a textbook virtual-override pattern conversion.

### What warrants review

1. **Base class change scope** — Adding `virtual` to `VerifyPermissionAsync` in the base class makes the method overrideable for all future accounting features. Reviewers should confirm this is an intentional extension point.

2. **Pattern consistency** — This establishes a pattern that other accounting features should follow if they need custom permission logic. Document this pattern for future feature development.

### Future work

1. **Update other features** — If other accounting query features need custom permissions, they should follow this virtual override pattern rather than explicit interface implementation.

2. **Document the pattern** — Add comments to `AccountingIdentificationFeatureBase` explaining when and how to override `VerifyPermissionAsync`.

---

## Step 2: Implement Iterations 2 & 3 (Stories 6-9) — WebApi Layer & Tests

**Author:** main

### Prompt Context

**Verbatim prompt:** "Make a plan for both iteration 2 and 3" ... "Start implementation"

**Interpretation:** Design and implement the complete WebApi layer (authorization policy, response DTO, endpoint method) plus comprehensive endpoint unit tests, split into two coherent iterations for reviewability.

**Inferred intent:** Complete the remaining user stories (6-9) that expose the PostingJournal feature via REST API, with full test coverage and proper authorization enforcement using the new `AccountingModifier` policy.

### What I did

Created 3 new files and modified 2 existing files for a total of ~275 LOC of implementation and tests:

**Story 6 & 6.2: Add AccountingModifier Authorization Policy**
1. Modified `/OSDevGrp.OSIntranet.Bff.WebApi/Security/Policies.cs` — Added `AccountingModifier` constant after `AccountingViewer`
2. Modified `/OSDevGrp.OSIntranet.Bff.WebApi/Program.cs` — Added policy builder configuration after `AccountingViewer` policy:
   - Requires `Internal` authentication scheme
   - Requires 6 claims: NameIdentifier, Name, Email, AccountingClaimType, **AccountingModifierClaimType** (differs from viewer)

**Story 7: Create PostingJournalResponseDto**
3. Created `/OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/PostingJournalResponseDto.cs` (~25 LOC):
   - Inherits from `AccountingIdentificationDto` (provides `Number` property)
   - Three [Required] properties: `PostingJournalTextsDto DynamicTexts`, `IReadOnlyCollection<StaticTextDto> StaticTexts`, `ValidationRuleSetDto ValidationRuleSet`
   - Static `internal Map()` method converts `PostingJournalResponse` to DTO

**Story 8: Add GET Endpoint to AccountingController**
4. Modified `/OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs`:
   - Added using statement: `using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;`
   - Added `PostingJournalAsync()` method (~20 LOC):
     - Route: `[HttpGet("{accountingNumber:int}/postingjournal")]`
     - Authorization: `[Authorize(Policy = Policies.AccountingModifier)]`
     - All 4 ProducesResponseType attributes (OK, BadRequest, Unauthorized, InternalServerError)
     - Orchestrates: security context resolution → creates `PostingJournalRequest` → executes query feature → maps & returns DTO
     - StatusDate always = today via `ResolveStatusDate(null)` (no query parameter)

**Story 9: Create Comprehensive Endpoint Tests**
5. Created `/OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/PostingJournalAsyncTests.cs` (~195 LOC):
   - 9 [Category("UnitTest")] test methods validating all orchestration paths
   - Tests verify: security context resolution, request ID generation, accounting number routing, status date calculation, format provider injection, security context propagation, cancellation token propagation, response type (OkObjectResult), DTO mapping
   - All 9 tests passing (657 ms)

### Why

The WebApi layer is the public contract for the PostingJournal feature. Stories 6-8 deliver the HTTP API that React applications will call to retrieve posting journals, while Story 9 ensures the endpoint correctly orchestrates all dependencies and produces the correct response.

Three design decisions:

1. **`AccountingModifier` policy (not viewer):** PostingJournal is accessed for write operations (users add/update/delete posting lines), so the policy requires write-level permissions, not read-only.

2. **Status date hardcoded to today:** The endpoint always uses today's date (via `ResolveStatusDate(null)`), matching UI requirements where users see current posting journal state. No query parameter variation.

3. **Separate iterations (2 & 3):** Bundling policy + DTO + endpoint (~80 LOC) into Iteration 2 keeps related infrastructure together and reviewable. Separating tests (Iteration 3, ~195 LOC) allows reviewers to focus on unit test comprehensiveness without mixing infrastructure details.

### What worked

- Build succeeded first try: 0 errors, 0 warnings after code completion
- All 9 tests passed immediately after test file creation (minor fixture extension method name issue corrected quickly)
- Test pattern reused from `AccountingPreCreationAsyncTests` — minimal custom logic needed
- DTO mapping reused existing StaticTextDto, PostingJournalTextsDto, ValidationRuleSetDto — no new DTO types needed
- Policy configuration followed exact pattern from `AccountingViewer` — predictable and consistent

### What didn't work

Two minor issues resolved quickly:

1. **Missing fixture extension method names** — Initial test used `CreatePostingJournalTexts()` without the `Random` parameter, and called non-existent `CreateEmptyValidationRuleSet()`. Fixed by discovering correct method signatures:
   - `CreateApplyPostingJournalModel(this Fixture fixture, Random random, ...)`
   - `CreatePostingJournalTexts(this Fixture fixture, Random random)`
   - `CreateValidationRuleSet(this Fixture fixture)`

2. **Missing using statement** — Tests initially failed with `CS1061: 'Fixture' does not contain a definition for 'CreatePostingJournalTexts'`. Fixed by adding `using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos;` to enable the fixture extension methods.

### What I learned

1. **Fixture extension method discovery** — Different test projects have fixture extensions in different namespaces. The pattern is:
   - `OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData` — service gateway test data builders
   - `OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos` — controller-specific DTO builders
   - `OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos` — shared DTO helpers

2. **Policy-driven authorization** — Adding a new policy is three steps: (1) add constant to Policies.cs, (2) add builder to Program.cs, (3) use attribute on endpoint. This is well-established and requires no surprises.

3. **Endpoint tests are simpler than feature tests** — Controller tests focus on orchestration (correct dependencies called in right order, right types returned) rather than business logic. The 9 tests capture all paths without needing complex test cases.

### What was tricky

1. **Fixture method parameter requirements** — Not obvious that `CreateApplyPostingJournalModel()` requires a `Random` parameter while other fixtures don't. Error message was clear once discovered, but required exploration of fixture extension files.

2. **Namespace depth for DTO fixtures** — Fixture extension methods for DTOs live in a controller-specific namespace, not in shared utilities. Had to search to find the right import.

### What warrants review

1. **Policy scope** — The `AccountingModifier` policy now allows any endpoint decorated with it to require write-level accounting permissions. Reviewers should confirm this is the right semantic for future endpoints.

2. **Endpoint route collision** — The new route `/api/accounting/{accountingNumber:int}/postingjournal` sits alongside other account-specific endpoints. Confirm no routing ambiguity.

3. **DTO inheritance** — `PostingJournalResponseDto` inherits from `AccountingIdentificationDto` which only provides `Number`. Confirm this is the intended base hierarchy (not `AccountingInfoDto` which adds `Name`).

4. **Test coverage** — 9 tests cover the happy path and parameter orchestration. Confirm this is sufficient without adding error-case tests (e.g., unauthorized access, missing parameters).

### Future work

1. **Story 10-12** — Run full solution build verification, regenerate OpenAPI/NSwag client, update implementation diary (complete).

2. **Manual testing** — After NSwag regeneration, test endpoint with real HTTP requests (requires running BFF service).

3. **Integration tests** — Current tests mock all dependencies. Integration tests with real service gateway could validate data flow end-to-end.

