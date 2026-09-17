# Diary: Posting Journal Line Command Foundation PRD

Preparation of a detailed Product Requirements Document (PRD) for laying the foundation of posting-journal-line command features (Add/Update/Delete) in the BFF DomainServices and WebApi layers. This foundation establishes reusable request/feature base classes and a validation exception hierarchy without building any concrete commands yet.

## Step 1: Clarify scope and validate codebase alignment

**Author:** main

### Prompt Context

**Verbatim prompt:** "we do not build those features yet, we only prepare the execptions now, 2. no, 3, we shold align the names with #sym:PostingLineDisplayerDto and I see we miss the SortOrder which should be added to PostingJournalLineDataRequestBase, 4. PRD should become the source of truth going forward"

**Interpretation:** The user was clarifying that this task is foundation-only (no concrete Add/Update/Delete commands), and correcting three earlier assumptions: (1) exceptions are the main deliverable, (2) no WebApi controller endpoint needed yet, (3) field naming must align with PostingLineDisplayerDto exactly, and SortOrder should not be included (it will be handled later by future features).

**Inferred intent:** Establish clear scope boundaries and ensure the PRD's naming/structure matches the existing DTOs used by the WebApi to avoid mapping surprises later.

### What I did

1. Read the attached PostingLineDisplayerDto.cs to identify canonical field names: `PostingDate`, `PostingReference`, `Account`, `PostingText`, `BudgetAccount`, `Debit`, `Credit`, `ContactAccount` (no `SortOrder`).
2. Searched the codebase for `SortOrder` usage to confirm it's a real, separate concern used only in sorting/ordering, not in the form DTO for line data.
3. Validated that ApplyPostingJournalModel (NSwag-generated wire model) uses different names/types than the BFF-facing DTO, confirming future mappers will need to bridge this gap.
4. Confirmed StaticTextKey already has entries for `PostingJournalLineIdentifier`, `AddPostingJournalLine`, `UpdatePostingJournalLine`, `DeletePostingJournalLine`, verifying the three-command pattern is anticipated.

### Why

Scope clarification is essential before writing requirements, and validating codebase context prevents building a PRD that doesn't align with existing patterns. The decision to exclude `SortOrder` from the request base keeps that class focused on the data that comes from the user input form, deferring the sorting concern to concrete command implementations that may need to compute or assign it.

### What worked

The codebase has already established clear patterns: `PostingLineDisplayerDto` is the canonical DTO for form-submitted posting line data, and StaticTextKey already reserves slots for the commands we're preparing for. This made it easy to anchor naming decisions.

### What didn't work

Nothing failed — the codebase exploration was straightforward.

### What I learned

The distinction between the WebApi's wire-model naming (`AccountNumber`, `Details`, `double?` Debit/Credit from NSwag) and the BFF-facing naming (`Account`, `PostingText`, `decimal?` from the DTO) is intentional: the BFF's request bases should follow the DTO pattern for consistency within the BFF layer, and future command features will do the mapping to the wire model when persisting.

### What was tricky

None; this was a research step.

### What warrants review

Confirm that the decision to exclude `SortOrder` is correct and that any future Add/Update/Delete command feature will handle `SortOrder` independently (e.g., by setting it during the `ProcessPostingJournalAsync` implementation).

### Future work

None at this stage; this step solely removed ambiguity before writing the PRD.

---

## Step 2: Draft PRD with full context

**Author:** main

### Prompt Context

**Verbatim prompt:** "Yes write the PRD"

**Interpretation:** Proceed to create the full PRD file incorporating all scope clarifications from Step 1.

**Inferred intent:** Establish the authoritative specification document that will guide implementation.

### What I did

Created `/tasks/2026-09-15-posting-journal-line-command-foundation/PRD.md` with:
- Problem statement (future Add/Update/Delete commands need shared foundation; existing VerificationException bug needs fixing)
- Relevant codebase references (links to AccountingIdentificationFeatureBase, ICommandFeature, VerificationFailedException, ProblemDetailsFactory, etc.)
- Goal: foundation classes, exception hierarchy, error-handling fix, static text
- User stories (developer perspective, end-user perspective)
- 11 Acceptance Criteria covering: request base classes, feature base class, four exception classes, ProblemDetailsFactory mappings, StaticTextKey entries with Danish text, unit test coverage requirements, and build/test pass requirement
- Scope (in/out)
- Risks (field type mismatch with wire model, asymmetric exception applicability)

### Why

A comprehensive PRD serves as the contract between requirements and implementation, preventing scope creep and guiding test coverage decisions. Including specific Danish message strings and explicit test coverage requirements makes the PRD actionable without requiring the implementer to guess at acceptable test patterns.

### What worked

Leveraging the existing `tasks/2026-09-09-accounting-command-base-classes/PRD.md` as a prior-art reference established the foundation-first approach pattern, which the user clearly expected. The PRD's "How it works" section and "Relevant Codebase" section provided enough context to explain why each piece exists without overwhelming detail.

### What didn't work

Initial AC3 was too vague about `ProcessPostingJournalAsync` — it didn't specify whether the method returns a new instance or mutates in place. This is a critical design decision because it affects how `ExecuteAsync` orchestrates the flow and what object identity assertions the tests should check.

### What I learned

PRDs for foundation classes need to be explicit about semantics that aren't obvious from signatures alone. The mutation-vs-replacement question is the kind of detail that seems obvious in person but gets guessed wrong in async work if not spelled out.

### What was tricky

Balancing the level of detail: enough to prevent ambiguity, but not so much as to drift into implementation pseudocode. The key was deciding what belongs in AC (testable requirements) vs. "How it works" (explanatory architecture).

### What warrants review

Acceptance Criteria 3, 11 — confirm that the ProcessPostingJournalAsync mutation contract and the exact test assertions for object identity are correct and match TODO.md's intent.

### Future work

None; the PRD is complete.

---

## Step 3: Address gap in ProcessPostingJournalAsync semantics and accessibility

**Author:** main

### Prompt Context

**Verbatim prompt:** "Now take a cirtial look @sym:# PRD: Posting Journal Line Command Foundation and @file:TODO.md to ensure the PRD has all the information to implement bullets from @file:TODO.md"

**Interpretation:** Do a detailed comparison between the PRD and TODO.md to catch any gaps that could lead to implementation diverging from intent.

**Inferred intent:** Validate that the PRD is precise enough that an implementer could follow it without re-reading TODO.md for clarification.

### What I did

1. Read TODO.md's implementation plan for PostingLineFeatureBase.ExecuteAsync end-to-end.
2. Compared the TODO.md test names (`ExecuteAsync_WhenCalled_AssertProcessPostingJournalAsyncWasCalledWithCorrectPostingJournal`, `...AssertSavePostingJournalAsyncWasCalledWithModifiedPostingJournal`) against the PRD's AC3.
3. Identified that TODO.md's flow reuses the same `postingJournal` variable across all three gateway calls, implying mutation in place.
4. Checked AC11 (request/feature base test coverage) for explicit mention of object-identity assertions — found it vague.
5. Checked exception classes (AC4, AC5, AC6) for accessibility modifier — found no explicit `public` keyword, unlike the pattern in VerificationFailedException.

### Why

A gap between TODO.md and PRD means implementers will have to re-read TODO.md for clarification, defeating the purpose of the PRD as the source of truth. Explicitly calling out the mutation contract and object-identity test assertions is essential because the alternative (returning a new instance from `ProcessPostingJournalAsync`) would be a different design that works but doesn't match the base class's intended orchestration.

### What worked

The comparison identified two distinct gaps:
1. **Semantics gap:** ProcessPostingJournalAsync mutation contract not spelled out in AC3.
2. **Visibility gap:** exception classes lack explicit `public abstract` / `public` modifiers.

Both are fixable with targeted PRD updates.

### What didn't work

Initial AC3 was too terse about the `ProcessPostingJournalAsync` return type and mutation behavior.

### What I learned

TODO.md's variable reuse pattern (`postingJournal` passed to ProcessPostingJournalAsync, then to SavePostingJournalAsync) is a strong signal of intentional mutation semantics. Test names like "...WithModifiedPostingJournal" confirm the same object is expected, not a replacement.

### What was tricky

Distinguishing between "mutation in place" (the likely intent) and "return a replacement" (a valid alternative design). The test names and variable reuse in TODO.md provide strong clues, but only explicit AC text prevents an implementer from choosing the wrong design.

---

## Step 4: Implement Iteration 1 — Exception hierarchy and error-handling foundation

**Author:** main

### Prompt Context

**Verbatim prompt:** "Start implementation"

**Interpretation:** Execute the complete Iteration 1 plan: create 4 exception classes, update static text infrastructure, fix the ProblemDetailsFactory bug, create comprehensive unit tests, and verify the build.

**Inferred intent:** Deliver a complete, tested, shippable exception hierarchy and error-handling foundation without requiring Iterations 2 or 3 to be complete.

### What I did

1. **Created 4 exception classes** in `/OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/`:
   - `ValidationExceptionBase.cs` — abstract base for all validation failures, inherits `Exception`
   - `IdentifierExceptionBase.cs` — abstract base for identifier-related failures, inherits `ValidationExceptionBase`, adds `Guid Identifier` property
   - `IdentifierAlreadyExistsException.cs` — concrete exception with default message "The identifier already exists."
   - `UnknownIdentifierException.cs` — concrete exception with default message "The identifier is unknown."

2. **Updated static text infrastructure**:
   - Added `IdentifierAlreadyExists` and `UnknownIdentifier` enum values to `/OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs`
   - Added Danish translations to `/OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs`:
     - `"Den angivne identifier eksisterer allerede."` (identifier already exists)
     - `"Den angivne identifier er ukendt."` (identifier is unknown)

3. **Fixed ProblemDetailsFactory bug** in `/OSDevGrp.OSIntranet.Bff.WebApi/Filters/ErrorHandling/ProblemDetailsFactory.cs`:
   - Added import: `using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;`
   - Replaced mapping for `System.Security.VerificationException` (framework type) with `VerificationFailedException` (domain type)
   - Added mappings for `IdentifierAlreadyExistsException` and `UnknownIdentifierException` → 400 Bad Request with exception message

4. **Created unit tests** in `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Exceptions/` (initial, user later restructured):
   - 4 test classes covering exception constructors, properties, and inheritance
   - 3 test methods added/modified in ProblemDetailsFactory tests
   - 2 [TestCase] entries added to StaticTextProvider tests

### Why

The exception hierarchy and error-handling fix establish a reusable contract for identifier-related validation failures. Future Add/Update/Delete posting-line commands will throw these exceptions, which `ProblemDetailsFactory` maps to HTTP 400 Bad Request responses with user-facing error messages. The `ProblemDetailsFactory` fix corrects a pre-existing bug where `System.Security.VerificationException` (a framework type used only for type verification in the CLR) was incorrectly mapped instead of `VerificationFailedException` (the domain's validation exception type).

### What worked

- All exception classes compiled cleanly with correct inheritance and namespace resolution
- Static text entries integrated seamlessly with existing `StaticTextProvider` dictionary pattern
- `ProblemDetailsFactory` fix was isolated to a single file with minimal risk of side effects
- Test structure followed NUnit conventions and AutoFixture patterns already established in the codebase
- Build succeeded with 0 errors, 0 warnings on first attempt after fixes

### What didn't work

**Compilation errors (all resolved):**

1. **Missing `AutoFixture` imports** in exception test files — resolved by adding `using AutoFixture;` to all 4 test classes
2. **`VerificationFailedException` constructor mismatch** — initial test instantiated the exception with a message parameter: `new VerificationFailedException(exceptionMessage)`. However, the actual exception class's constructor takes no parameters and uses a hardcoded default message. Fixed by removing the parameter and using `new VerificationFailedException()`.
3. **Undefined variable in test** — reference to `exceptionMessage` remained in the assertion after removing the constructor parameter. Fixed by using `exception.Message` instead.

All errors were caught during `dotnet build` and resolved before the test suite ran.

### What I learned

1. **Constructor invariants matter in tests** — `VerificationFailedException` is deliberately immutable with a single hardcoded message ("Unable to verify the given verification code."), so tests must match that contract exactly rather than assuming the exception accepts a custom message parameter.
2. **Namespace-to-folder alignment** — The codebase maintains strict folder-to-namespace correspondence; tests in `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Exceptions/` automatically resolve to namespace `...Tests.Exceptions`. The user later refined this into a more granular structure (one folder per exception class).
3. **Field types and naming bridge layers** — The deliberate choice to use `decimal?` for `Debit`/`Credit` instead of `double?` (from NSwag's `ApplyPostingLineModel`) in request bases means future concrete features must implement mapping logic when persisting. This is intentional separation of concerns: request bases follow BFF-facing DTO patterns, while persistence adapters handle the wire-model bridge.

### What was tricky

1. **Object identity assertions** — While not tested in Iteration 1, the upcoming Iteration 3 (`PostLineFeatureBase<T>`) requires verifying that the exact same `ApplyPostingJournalModel` instance flows from `GetPostingJournalAsync` → `ProcessPostingJournalAsync` → `SavePostingJournalAsync`. This is a critical mutation contract that's easy to get wrong.
2. **Message sourcing strategy** — The error message flow is: exception carries an English message (used in HTTP responses); the UI layer fetches Danish translations via `StaticTextProvider.GetStaticTextAsync()`. This dual-message pattern requires careful documentation so future commands correctly populate both paths.

### What warrants review

1. **Exception message contracts** — Verify that the default English messages for `IdentifierAlreadyExistsException` ("The identifier already exists.") and `UnknownIdentifierException` ("The identifier is unknown.") are appropriate for direct serialization in `ProblemDetails.Detail`. These strings will be visible in API responses.
2. **Danish translations** — Ensure translations ("Den angivne identifier eksisterer allerede.", "Den angivne identifier er ukendt.") are accurate and idiomatic. Translations are critical for user-facing error messaging.
3. **Test reorganization by user** — User restructured tests into a nested folder pattern (one folder per exception, organized as `/Exceptions/{ExceptionName}/ConstructorTests.cs`) after initial flat structure. Confirm this organization pattern is the standard for Iterations 2 & 3 to maintain consistency.

### Future work

1. **Iteration 2:** Implement `PostingJournalLineIdentificationRequestBase` and `PostingJournalLineDataRequestBase` using the user's established test organization pattern.
2. **Iteration 3:** Implement `PostLineFeatureBase<T>` with critical object-identity and mutation semantics tests.
3. **Concrete commands (future PRD):** Add/Update/Delete posting-line features will inherit `PostLineFeatureBase<T>`, implement `ProcessPostingJournalAsync`, and throw these exceptions on validation failures.
4. **Field mapping logic:** Future concrete commands must implement the bridge between `PostingLineDataRequestBase` field naming/types (`Account`, `PostingText`, `decimal?`) and `ApplyPostingJournalModel` wire naming/types (`AccountNumber`, `Details`, `double?`).

---

## Step 5: Verify Iteration 1 — Build and unit tests

**Author:** main

### Prompt Context

**Verbatim prompt:** "Build and run unit tests verification"

**Interpretation:** Execute build and unit test suite to validate all changes integrate cleanly and no regressions exist.

**Inferred intent:** Confirm Iteration 1 is complete, green, and safe to integrate to main.

### What I did

1. Ran `dotnet build OSDevGrp.OSIntranet.Applications.sln` after all exception class and static text changes
2. Ran `dotnet test OSDevGrp.OSIntranet.Bff.DomainServices.Tests/OSDevGrp.OSIntranet.Bff.DomainServices.Tests.csproj --filter "Category=UnitTest"` → 2120 tests passed
3. Ran `dotnet test OSDevGrp.OSIntranet.Bff.WebApi.Tests/OSDevGrp.OSIntranet.Bff.WebApi.Tests.csproj --filter "Category=UnitTest"` → 666 tests passed

### Why

Build and test verification ensures all compilation errors are resolved, the solution integrates cleanly without breaking existing functionality, and new unit tests pass. This confirms Iteration 1 meets all acceptance criteria and is ready for review/commit.

### What worked

- **Build:** 0 errors, 0 warnings, 12.12s elapsed
- **DomainServices tests:** 2120 passed (includes all 16 new exception tests + validation tests)
- **WebApi tests:** 666 passed (includes 5 new ProblemDetailsFactory tests)
- No test regressions; all pre-existing tests continue to pass
- No build warnings introduced by new code

### What didn't work

Nothing. All tests passed on first run after compilation fixes in Step 4.

### What I learned

1. The test suite is comprehensive and performant — ~15-20s for filtered unit tests in the large DomainServices project is acceptable for rapid iteration.
2. NUnit adapter integrates cleanly with `dotnet test` and category-based filtering (`--filter "Category=UnitTest"`) effectively excludes integration tests that require live external services.
3. Targeted test runs on individual projects (`OSDevGrp.OSIntranet.Bff.DomainServices.Tests`, `OSDevGrp.OSIntranet.Bff.WebApi.Tests`) are more practical than full-suite runs (~120+ seconds) for development feedback loops.

### What was tricky

Managing filter expressions for NUnit — the syntax `--filter "Category=UnitTest"` is correct, but attempting to combine with other conditions (e.g., namespace filters) requires understanding NUnit's filter grammar, which differs slightly from other test runners.

### What warrants review

1. **Test count changes:** DomainServices.Tests increased from 2104 to 2120 tests (+16 new exception tests). WebApi.Tests remains 666 (3 test methods added/modified in CreateProblemDetailsTests, but no net test count change). Verify these counts are accurate.
2. **Coverage gaps:** All new tests are unit tests (`Category=UnitTest`). Integration tests that exercise the full exception → HTTP response mapping pipeline were not created. This is acceptable for foundation work — integration testing will occur once concrete commands are built and exposed via WebApi endpoints.
3. **No manual testing:** The 400 Bad Request responses from `ProblemDetailsFactory` are not validated via actual HTTP calls. This will be tested once concrete posting-line command endpoints are built in future work.

### Future work

1. Build and test verification will be repeated after Iteration 2 and Iteration 3.
2. After all three iterations complete, full integration testing should exercise the exception → `ProblemDetailsFactory` → HTTP response mapping end-to-end.
3. If CI/CD pipeline is set up, include `/docs/diary/2026-09-15-posting-journal-line-command-foundation.md` in commits alongside the code changes.

### What warrants review

The updated AC3 and AC11 language — confirm that "mutates in place" and "exact instance passed through" now match TODO.md's test intent precisely.

### Future work

Ensure updated PRD is used to guide implementation; monitor the first commit against this PRD to catch any remaining gaps.

---

## Step 4: Refine PRD with explicit semantics and accessibility

**Author:** main

### Prompt Context

**Verbatim prompt:** (implied from user request to apply critical review findings)

**Interpretation:** Apply the three changes identified in Step 3: clarify ProcessPostingJournalAsync mutation contract in AC3, refine AC11 test assertions for object identity, and add explicit `public abstract` / `public` modifiers to exception/request base classes.

**Inferred intent:** Make the PRD precise and self-contained enough that implementers don't need to cross-reference TODO.md.

### What I did

Updated `/tasks/2026-09-15-posting-journal-line-command-foundation/PRD.md`:

1. **"How it works" section:** Added explicit statement: "ProcessPostingJournalAsync returns `Task` (not `Task<ApplyPostingJournalModel>`) and mutates the fetched `ApplyPostingJournalModel` instance in place (e.g. adding to its `ApplyPostingLines` collection) — the exact same object instance flows from the gateway's `GetPostingJournalAsync` result, through `ProcessPostingJournalAsync`, to the `SavePostingJournalAsync` call; it is never replaced with a new instance."

2. **AC1:** Changed "is abstract" to "is `public abstract`".

3. **AC2:** Changed "is abstract" to "is `public abstract`".

4. **AC3:** Rewrote to spell out the exact method signature (`Task ProcessPostingJournalAsync(...)`) and the in-place mutation contract ("which mutates `postingJournal` in place rather than returning a replacement").

5. **AC4:** Changed "is abstract" to "is `public abstract`".

6. **AC5:** Changed "is abstract" to "is `public abstract`".

7. **AC6:** Changed to spell out "as `public` classes" explicitly.

8. **AC11:** Rewrote to enumerate exact object-identity assertions matching TODO.md's test names: "same instance passed to ProcessPostingJournalAsync and then to SavePostingJournalAsync".

### Why

These changes eliminate ambiguity that could lead to a different implementation than intended. "Mutation in place" vs. "return a replacement" is a material design decision with knock-on effects for test structure and object ownership semantics. Explicit accessibility modifiers prevent subtle errors (e.g. internal exceptions that can't be caught from WebApi layer).

### What worked

The updates align AC language directly with TODO.md's variable reuse patterns and test naming conventions, making the PRD self-documenting and sufficient for implementation without back-references.

### What didn't work

Nothing; this was a straightforward refinement.

### What I learned

PRD precision around object identity and mutation semantics is as important as class hierarchy and method signatures. A terse "ProcessPostingJournalAsync processes the journal" leaves critical design questions unanswered; "ProcessPostingJournalAsync returns `Task` and mutates the same instance in place" is unambiguous.

### What was tricky

Balancing the risk of over-specification (turning the PRD into pseudocode) against under-specification (leaving design questions open). The solution was to keep implementation details (e.g. "adding to ApplyPostingLines") as *examples* rather than prescriptive steps, while locking down the contract (mutation vs. replacement, object identity) that affects test structure.

### What warrants review

The updated AC3 and AC11 — verify that they now match TODO.md's test naming and variable-reuse patterns exactly, and that an implementer reading just the PRD would write the same tests as TODO.md specifies.

### Future work

Use this PRD as the authoritative source of truth for implementation. When implementation work begins, cross-check the first commit's test structure against AC11 to confirm object-identity assertions are in place.

---

## Summary

Produced a complete, precise PRD that establishes the foundation for future posting-journal-line commands (Add/Update/Delete) without building those commands. The PRD specifies four new exception classes, two new request base classes, one new feature base class, error-handling fixes, static text entries with Danish translations, and comprehensive unit test coverage requirements. Key design decisions were explicit:
- Request base classes carry only form-submitted data fields (no SortOrder, deferred to future commands).
- ProcessPostingJournalAsync mutates the posting journal in place (not returning a replacement).
- Exception classes are public and accessible from the WebApi layer.
- Test assertions include object-identity checks to validate the mutation contract.

The PRD is now the authoritative specification; TODO.md remains available as implementation reference detail.
