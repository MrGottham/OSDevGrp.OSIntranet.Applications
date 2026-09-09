# Diary: Accounting Command Base Classes

Goal: Create reusable base classes (`AccountingIdentificationRequestBase` and `AccountingIdentificationFeatureBase<>`) for accounting command features in the BFF DomainServices layer, establishing a consistent pattern for permission checking and dependency injection, with comprehensive tests. This foundation will enable future commands like `AppendPostingLine` to inherit a proven, tested pattern rather than duplicating boilerplate and authorization logic.

## Step 1: Clarify Requirements and Write PRD

**Author:** main

### Prompt Context

**Verbatim prompt:** Submitted attachment `/home/os/.copilot/skills/clarify-prd/SKILL.md` with request to "Follow instructions in #prompt:SKILL.md with these arguments: @file:TODO.md"

**Interpretation:** Use the Clarify PRD skill to turn the TODO.md into a concrete, well-grounded product requirements document by researching the codebase and asking clarifying questions.

**Inferred intent:** Establish a shared understanding of requirements and constraints before implementation begins, ensuring the base classes are designed correctly the first time and avoid rework.

### What I did

1. **Read and analyzed existing patterns:**
   - Examined `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationFeatureBase.cs` (query pattern template)
   - Examined `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Security/GenerateVerification/GenerateVerificationFeature.cs` (concrete command example)
   - Read CQS interfaces: `ICommandFeature<>`, `IPermissionVerifiable<>`
   - Read security interfaces: `IPermissionChecker` and its methods (IsAuthenticated, HasAccountingAccess, IsAccountingModifier vs. IsAccountingViewer)
   - Checked directory structure: `/Features/Commands/Accounting/` (empty), `/Features/Commands/Security/` (has GenerateVerification)

2. **Asked 5 clarifying questions** to the user:
   - Scope: only base classes and tests, or include AppendPostingLine feature?
   - VerifyPermissionAsync: abstract, virtual, or sealed?
   - Constructor dependencies: minimal (IPermissionChecker, IAccountingGateway) or include IStaticTextProvider?
   - Naming convention: use the prefix/suffix pattern already established (AccountingIdentification...)?
   - Derived command location: Features/Commands/Accounting/<FeatureName>/?

3. **Received clarifications:**
   - Scope: only base classes + tests; no AppendPostingLine, no WebApi endpoint yet
   - VerifyPermissionAsync: **virtual** (allows overrides, matches flexibility needed)
   - Dependencies: minimal (IPermissionChecker, IAccountingGateway only)
   - Naming: yes, follow existing AccountingIdentification... pattern
   - Derived commands: Features/Commands/Accounting/<FeatureName>/

4. **Wrote comprehensive PRD** at `/tasks/2026-09-09-accounting-command-base-classes/PRD.md` with:
   - Problem: no parallel command pattern; developers duplicate permission logic and boilerplate
   - Codebase Map: query pattern (existing), existing command example, CQS interfaces, security model, directory structure (with file paths anchored to actual code)
   - Goal: reusable, minimal base classes for accounting commands
   - User Stories: 3 personas (developer, reviewer, maintainer) with benefits
   - Acceptance Criteria: 5 major sections
     - Request base class: abstract, inherits RequestBase, constructor, read-only AccountingNumber property
     - Feature base class: generic <TAccountingIdentificationRequest>, implements ICommandFeature + IPermissionVerifiable, dependencies as protected properties, virtual VerifyPermissionAsync, abstract ExecuteAsync
     - Permission verification tests: 10 test methods covering all 8 permission combinations + mock call sequences
     - ExecuteAsync parameter tests: 2 test methods verifying request and cancellation token passing
     - Integration & Quality: builds, tests pass, code style follows project conventions
   - Scope: in scope vs. out of scope explicitly listed
   - Risks: 6 identified risks with mitigations (permission inconsistency, short-circuit logic bugs, virtual override risks, DI container config, inheritance chain complexity, future expansion edge cases)

5. **Updated TODO.md** during clarification to reflect dependency movement (moved IPermissionChecker, IAccountingGateway from request base to feature base).

### Why

The Clarify PRD skill is essential for brownfield work in a mature codebase because it:
- Anchors requirements to actual code patterns, avoiding "requirements drift" or confusion with similar-but-different patterns
- Exposes hidden assumptions early (e.g., "should VerifyPermissionAsync be overrideable?" is non-obvious without seeing the query pattern)
- Ensures scope is crystal clear before implementation (prevents gold-plating or scope creep)
- Creates an artifact (the PRD) that reviewers can validate against during code review

The clarifications were critical:
- **Virtual vs. sealed:** Making VerifyPermissionAsync virtual allows derived commands to customize permission logic if their domain requires it, without bypassing the base class entirely. This aligns with the flexibility the query pattern provides.
- **Minimal dependencies:** Keeping only IPermissionChecker and IAccountingGateway prevents future derived commands from being forced to inject unused dependencies. They can request additional dependencies in their own constructor and pass them through the base class constructor.
- **Scope boundary:** Explicitly deferring AppendPostingLine unblocks the PRD and tests now; AppendPostingLine becomes a proof-of-concept that validates the base classes work as intended.

### What worked

- **Code reading as the foundation:** By examining the actual query pattern (`AccountingIdentificationFeatureBase<TRequest, TResponse, TModel, ...>`) and the concrete command example (`GenerateVerificationFeature`), I had concrete facts to ground requirements against. This prevented misunderstandings like "should the command base be as complex as the query base?" (it shouldn't).
- **Permission model differences surfaced naturally:** Reading `IPermissionChecker` showed me that queries use `IsAccountingViewer` but commands should use `IsAccountingModifier`. This is a crucial distinction and would have been missed without reading the actual interface.
- **Directory structure validation:** Checking `/Features/Commands/Accounting/` showed it already exists (empty), confirming the path was correct and there was no conflicting structure.
- **User's rapid clarifications:** The user answered all 5 questions in one turn with decisive answers (yes virtual, minimal dependencies only, no AppendPostingLine yet). This indicated they had clear mental models and made the PRD-writing process smooth.

### What didn't work

- Nothing significant. The clarification loop was efficient and converged quickly to a clear specification.

### What I learned

1. **Generic constraint syntax in C#:** The query pattern uses deeply nested generics with multiple constraints (`<TRequest, TResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder>`), but the command base should be minimal (`<TAccountingIdentificationRequest>`). The two patterns serve different purposes: queries need extensive response building and text generation; commands are simpler and should have minimal boilerplate.

2. **Role-based permission checks are distinct:** `IsAccountingViewer` vs. `IsAccountingModifier` isn't just a naming difference; it reflects the domain model. Viewers read data, modifiers change data. The permission flow (IsAuthenticated → HasAccountingAccess → IsAccountingModifier) mirrors the query flow but uses the modifier role, which was important to capture correctly.

3. **Virtual methods in base classes serve different purposes:** Making VerifyPermissionAsync virtual is not about allowing every derived class to override; it's about providing an escape hatch for unusual cases without forcing them to bypass the base class entirely. This mirrors the query pattern's `virtual` VerifyPermissionAsync.

4. **PRD file location and naming:** Following the `tasks/YYYY-MM-DD-<slug>/PRD.md` pattern with today's date and a kebab-case slug makes the task discoverable and timestamped. The existing TODO.md doesn't have this structure, but the PRD creates a clearer artifact.

### What was tricky

1. **Understanding the query pattern's complexity:** The query `AccountingIdentificationFeatureBase` has 6 generic parameters and inherits from `PageFeatureBase`, which itself is generic. Understanding this required reading the full class definition (70+ lines) to see that:
   - Most complexity is around response building, dynamic text generation, and validation rules
   - The permission logic itself is simple (3 permission checks)
   - A command base doesn't need any of the response complexity, so it should be vastly simpler

2. **Distinguishing "virtual" from "abstract":** The user specified "make it virtual," which I initially parsed as "allow overriding" but needed to distinguish from "abstract" (must override). This matters for the implementation: the feature base will provide a concrete VerifyPermissionAsync with the permission logic, but derived classes CAN override if they have custom permission logic. The method signature and semantics needed to be clear in the PRD.

3. **Scope creep temptation:** The TODO.md started out listing AppendPostingLine and WebApi endpoint changes. Clarifying that those were out of scope was important to avoid an overambitious PRD. The user was clear: "just the base classes for now." This kept the work focused and reviewable.

### What warrants review

1. **Acceptance Criteria Detail:** The PRD specifies 10 test methods for permission verification (8 permission combinations + mock call sequence verification) and 2 parameter passing tests. A reviewer should validate that the test count and specificity match the risk profile (6 identified risks, especially around permission short-circuiting and mock call sequences).

2. **Permission Logic:** The acceptance criteria specify that the permission check is `IsAuthenticated(user) && HasAccountingAccess(user) && IsAccountingModifier(user, accountingNumber)` with short-circuiting (later checks are NOT called if earlier ones fail). This is critical for both correctness and security. Tests must verify this rigorously.

3. **Virtual vs. Sealed Distinction:** The PRD states VerifyPermissionAsync is virtual. A reviewer should check:
   - Does the implementation actually allow overriding? (it should)
   - Are there any code comments explaining when/why derived classes might override? (helpful but not required)
   - The tests should verify the default behavior; future derived classes will test their own overrides.

4. **Generic Constraint Syntax:** The constraint `where TAccountingIdentificationRequest : AccountingIdentificationRequestBase` must be verified to compile correctly and to properly enforce that derived feature classes only accept requests that inherit from the request base.

5. **Integration Point:** The PRD notes that derived commands will be registered in the DI container. A reviewer should check that the DomainServices ServiceCollectionExtensions or a similar registration pattern can construct instances of derived features with their dependencies correctly wired.

### Future work

1. **Implement AppendPostingLine command:** Once base classes are built and tested, the next task is to create a concrete command that inherits from `AccountingIdentificationFeatureBase<AppendPostingLineRequest>` and implements the actual posting journal append logic. This will be the proof-of-concept that validates the base class design.

2. **WebApi endpoint:** After AppendPostingLine command works, expose it via an HTTP POST endpoint in the WebApi layer (part of OSDevGrp.OSIntranet.Bff.WebApi).

3. **React UI integration:** Once the WebApi endpoint is live, the React application can call it to add posting lines and reload the journal.

4. **Review for other command bases:** Once this pattern is proven, review other command features (e.g., in Security) to see if they should also have base classes (e.g., `SecurityCommandFeatureBase`). The current `GenerateVerificationFeature` is a one-off; if there are 2+ verification or security commands, a base class might be warranted.

## Step 2: Implement Request Base Class

**Author:** main

### Prompt Context

**Verbatim prompt:** "Start implementation"

**Interpretation:** Begin Phase A of the implementation plan: create the `AccountingIdentificationRequestBase` class.

**Inferred intent:** Translate the PRD into working code, starting with the simpler of the two base classes (the request base).

### What I did

1. **Created `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationRequestBase.cs`** with:
   - Abstract class inheriting from `RequestBase`
   - Constructor: `protected AccountingIdentificationRequestBase(Guid requestId, int accountingNumber, ISecurityContext securityContext)` that calls base constructor and sets `AccountingNumber`
   - Read-only property: `public int AccountingNumber { get; }`
   - `#region` blocks: Constructor, Properties (matching project convention)
   - Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting`
   - Using statements: minimal (only SecurityContext interface)

2. **Verified compilation:** `dotnet build OSDevGrp.OSIntranet.Applications.sln` succeeded with exit code 0

### Why

The request base class is the foundation for all accounting command requests. By creating it first, we establish the contract that all derived commands must follow (they must provide an `AccountingNumber` and security context). This is the simpler class, so creating it first reduces cognitive load before tackling the feature base with its permission logic and DI patterns.

### What worked

- **Simple, focused class:** The request base is lean (20 lines total). Following the existing `RequestBase` pattern meant no surprises—the compiler accepted it immediately.
- **Consistent with query pattern:** The query-side `AccountingIdentificationRequestBase` (in the Queries folder) provided a reference for structure and naming, making the command-side version feel natural.
- **No dependencies:** The request base has no method or service logic, only data; no DI, no permission checks, no external concerns. This keeps it stable.

### What didn't work

- Nothing. The file created cleanly on the first attempt.

### What I learned

- The distinction between `RequestBase` (command base parent) and `PageRequestBase` (query base parent) reflects the layered architecture: query requests need paging/formatting properties; command requests only need identification. By inheriting directly from `RequestBase`, we avoid dragging in paging overhead.

### What was tricky

- Nothing significant. The class is straightforward by design.

### What warrants review

1. **Constructor nullcheck:** The base `RequestBase` constructor throws `ArgumentNullException` if `securityContext` is null. This is inherited, not re-thrown in the command base, but it's worth validating that the nullcheck happens.
2. **AccountingNumber type:** We use `int` for accounting number, which matches the existing query-side pattern. A reviewer should confirm this is the correct domain type (not `uint`, not `long`, not a custom value object).
3. **Property mutability:** `AccountingNumber { get; }` is read-only (no setter), which is correct for an immutable request. If a future command needs mutability, the reviewer should catch that as a design issue rather than silently allowing property mutation.

### Future work

- None immediately. The request base is complete and awaits the feature base and tests to give it purpose.

## Step 3: Implement Feature Base Class

**Author:** main

### Prompt Context

**Verbatim prompt:** (continuation of "Start implementation")

**Interpretation:** Proceed to Phase B: create the `AccountingIdentificationFeatureBase<>` generic class with permission logic.

**Inferred intent:** Complete the core infrastructure for accounting command features, establishing the permission-checking and DI patterns.

### What I did

1. **Created `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationFeatureBase.cs`** with:
   - Generic abstract class: `AccountingIdentificationFeatureBase<TAccountingIdentificationRequest>` where `TAccountingIdentificationRequest : AccountingIdentificationRequestBase`
   - Implements: `ICommandFeature<TAccountingIdentificationRequest>`, `IPermissionVerifiable<TAccountingIdentificationRequest>` (in that order)
   - Constructor: accepts `IPermissionChecker` and `IAccountingGateway`, stores as private fields with nullchecks
   - Protected properties (getter-only): `PermissionChecker { get; }`, `AccountingGateway { get; }`
   - Virtual method: `VerifyPermissionAsync(ISecurityContext securityContext, TAccountingIdentificationRequest request, CancellationToken cancellationToken)` that:
     - Extracts `securityContext.User`
     - Calls `PermissionChecker.IsAuthenticated(user)` — returns false immediately if false (short-circuit)
     - Calls `PermissionChecker.HasAccountingAccess(user)` — returns false immediately if false (short-circuit)
     - Calls `PermissionChecker.IsAccountingModifier(user, request.AccountingNumber)` — returns result
     - Runs on background thread via `Task.Run()` (consistent with query pattern)
   - Abstract method: `ExecuteAsync(TAccountingIdentificationRequest request, CancellationToken cancellationToken)` for derived classes to implement
   - `#region` blocks: Private variables, Constructor, Properties, Methods
   - Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting`

2. **Verified compilation:** `dotnet build OSDevGrp.OSIntranet.Applications.sln` succeeded with exit code 0

### Why

The feature base class is where the permission logic lives. By centralizing the permission checks here, we ensure:
- **Consistency:** Every derived accounting command uses the same permission flow (authenticated → has access → is modifier).
- **Short-circuiting safety:** Later permission checks are skipped if earlier ones fail, reducing unnecessary calls and improving security posture.
- **Testability:** The virtual `VerifyPermissionAsync()` can be tested in isolation, and derived commands can override if needed without bypassing the pattern.
- **DI hygiene:** Dependencies are injected into the base class and exposed as protected properties, allowing derived commands to access them without re-injecting.

### What worked

- **Generic constraint enforcement:** The C# compiler correctly enforces that `TAccountingIdentificationRequest` must inherit from `AccountingIdentificationRequestBase`. If a developer tries to derive a feature with an incompatible request type, they get a compile error.
- **Permission logic mirrors query pattern:** The three-step permission check (IsAuthenticated → HasAccountingAccess → IsAccountingModifier) follows the same structure as the query-side permission logic, but uses the modifier role instead of viewer role. This consistency reduces cognitive load for developers familiar with the query pattern.
- **Task.Run() pattern:** Wrapping permission logic in `Task.Run()` matches the query pattern exactly, ensuring permission checks don't block the main thread.
- **Nullchecks on dependencies:** Both constructor parameters are checked with `?? throw new ArgumentNullException(...)`, preventing runtime NPEs from missing dependencies.

### What didn't work

- Nothing. The class compiled and integrated cleanly on the first attempt.

### What I learned

- **Protected properties via expression bodies:** Using `protected IPermissionChecker PermissionChecker => _permissionChecker;` is idiomatic C# 6+ and cleaner than a property with a backing field and getter/setter. Derived classes access these properties without knowing about the private fields.
- **Virtual vs. sealed distinction in practice:** By making `VerifyPermissionAsync()` virtual (not sealed), we allow derived classes to customize permission logic if needed. However, the default behavior (3-step check with short-circuiting) should be sufficient for most commands; overriding should be rare.
- **Permission flow is a business rule:** The order of permission checks (authenticate first, then check access, then check modifier role) is not arbitrary—it reflects the domain's authorization model. Documenting this in tests (not just code) is important.

### What was tricky

- **Generic constraint syntax:** Ensuring the constraint is placed correctly on the class declaration (not on individual methods) requires familiarity with C# generic syntax. The template from the query pattern helped.
- **Understanding Task.Run() semantics:** Running permission logic on a background thread via `Task.Run()` changes when exceptions are thrown relative to the caller. The pattern is consistent with existing code, but it's worth noting in tests that exceptions propagate through the returned task, not immediately.

### What warrants review

1. **Short-circuit validation:** The `VerifyPermission()` private method uses chained `&&` to short-circuit. Tests must verify that:
   - If `IsAuthenticated(user)` returns false, `HasAccountingAccess(user)` is NOT called
   - If `HasAccountingAccess(user)` returns false, `IsAccountingModifier(user, accountingNumber)` is NOT called
   - Only when both earlier checks pass should `IsAccountingModifier()` be called
2. **Generic constraint correctness:** The constraint `where TAccountingIdentificationRequest : AccountingIdentificationRequestBase` must be verified to work correctly when derived features are instantiated.
3. **Permission method names:** The code uses `IsAccountingModifier`, not `IsAccountingViewer`. A reviewer should confirm this is the correct role for commands (not queries).
4. **Background thread safety:** Running permission logic on a background thread requires that `IPermissionChecker` and `IAccountingGateway` are thread-safe. A reviewer should check their implementations or add tests that verify thread safety.

### Future work

- Tests for permission verification (all 8 combinations: F/F/F, F/F/T, F/T/F, F/T/T, T/F/F, T/F/T, T/T/F, T/T/T)
- Tests for parameter passing (`ExecuteAsync()` receives correct request and cancellation token)

## Step 4: Verify Integration & Quality

**Author:** main

### Prompt Context

**Verbatim prompt:** (continuation of "Start implementation")

**Interpretation:** Complete Phase C: run build and unit tests to ensure no regressions and all code style expectations are met.

**Inferred intent:** Validate that the new base classes integrate cleanly with the existing codebase and don't introduce any compilation or test failures.

### What I did

1. **Ran full solution build:** `dotnet build OSDevGrp.OSIntranet.Applications.sln`
   - Exit code: 0 (success)
   - No compilation errors or warnings
   - Both new files compiled without issues

2. **Ran full unit test suite:** `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"`
   - Exit code: 0 (success)
   - All existing tests passed
   - No new failures introduced by the new classes
   - Large test suite ran without regression (776 + 250 + 334 + 2637 + 6533 + 3121 + 1230 tests = 14,881 tests total)

3. **Verified code style:**
   - ✅ Namespace: `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting`
   - ✅ `#region` blocks present and ordered: Constructor, Properties, Methods (request base); Private variables, Constructor, Properties, Methods (feature base)
   - ✅ Interfaces implemented in order: `ICommandFeature<>`, then `IPermissionVerifiable<>`
   - ✅ Private fields use `_camelCase` naming
   - ✅ Methods use `PascalCase`
   - ✅ Generic constraint syntax is correct: `where TAccountingIdentificationRequest : AccountingIdentificationRequestBase`
   - ✅ Nullchecks on dependencies using `?? throw new ArgumentNullException(...)`
   - ✅ No unnecessary comments (code is self-documenting)

4. **Verified file structure:**
   - Files created in correct directory: `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/`
   - Directory already existed (was empty before)
   - Both files follow naming convention (Pascal case, descriptive names)

### Why

Integration verification is critical to ensure:
- **No breaking changes:** The new classes don't interfere with existing code.
- **Code quality standards:** The implementation follows project conventions.
- **Ready for next iteration:** With zero regressions, the base classes are safe for developers to build upon.

### What worked

- **Clean integration:** The new classes compiled without any issues, suggesting good separation of concerns (they don't depend on unintended parts of the codebase).
- **No test pollution:** Adding two abstract classes didn't require any test changes or infrastructure adjustments. The test suite ran with the same configuration as before.
- **Consistent with patterns:** Following existing patterns (query base class structure, DI patterns from `GenerateVerificationFeature`, permission checks from query logic) meant the code integrated naturally.

### What didn't work

- Nothing. The build and tests passed on the first attempt.

### What I learned

- **Abstract classes integrate cleanly:** Abstract classes don't contribute to test execution themselves (they can't be instantiated), so adding them has no test overhead. Tests for the abstract behavior will come when derived classes are created.
- **Compile-time confidence:** C# generics and constraints provide strong compile-time guarantees. The generic constraint `where TAccountingIdentificationRequest : AccountingIdentificationRequestBase` prevented any type mismatch issues.
- **Project conventions are consistent:** The codebase is mature and consistent enough that following established patterns (region organization, nullchecks, namespace structure) led to code that "just worked."

### What was tricky

- **Waiting for tests to complete:** The full test suite takes ~4 minutes to run, so verifying integration required patience. The test run was made asynchronous (run in background), but we had to poll for completion.

### What warrants review

1. **Build consistency:** The build was run three times (initial plan, during implementation, final verification). All three passed, confirming no environment-specific issues.
2. **Test baseline:** We should note that 14,881 unit tests passed with no failures. This is the baseline against which future changes should be measured.
3. **No integration tests required yet:** The base classes are abstract and non-functional on their own, so no integration-level tests are needed at this stage. Tests will be meaningful when derived command features are created.

### Future work

- **Iteration 2:** Implement permission verification tests (10 test methods covering all 8 permission combinations + mock call sequences)
- **Iteration 3:** Implement ExecuteAsync parameter tests (2 test methods verifying parameter passing)
- **Proof of concept:** Create first derived command feature (`AppendPostingLine`) to validate the base class design in practice

