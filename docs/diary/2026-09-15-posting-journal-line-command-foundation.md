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
