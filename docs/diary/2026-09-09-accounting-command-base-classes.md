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

