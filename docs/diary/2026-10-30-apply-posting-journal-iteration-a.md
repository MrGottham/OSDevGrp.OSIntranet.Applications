# Diary: Apply posting journal, Iteration A (BFF gateway and command)

Iteration A of the "Apply posting journal with warning notifications" feature in `tasks/2026-10-28-apply-posting-journal/PRD.md`. It covers the BFF side only: the gateway forwards the apply request to the Web API, and the apply command saves the remaining draft and publishes posting warnings. There is no HTTP endpoint and no React component yet. The previous iteration (notification infrastructure) is already in `docs/diary/2026-10-29-apply-posting-journal.md`.

## Step 1: Gateway, apply command, static text, and tests

**Author:** main

### Prompt Context

**Verbatim prompt:** /suggest-next-iteration

Followed by: "Lets plan iteration A"

Followed by: "When implementing unit testing the make sure to make test methods testing only one thing. Use the CreateSut pattern and make the mocking Setup within the CreateSut When making the integration test for ApplyPostingJournalAsync make sure to test with two posting lines with a debit value in the first on and the same value as credit in the second one. We gona ignore this integration test when we know the functionality is working"

**Interpretation:** Pick the next slice of the PRD, then implement it with one behavior per test method, mock setup in `CreateSut` helpers, and an apply integration test that uses two lines (a debit, then an equal credit).

**Inferred intent:** Get the BFF apply path built and tested without HTTP wiring, and keep the test style consistent for the rest of the feature.

### What I did

- Added `ApplyPostingJournalAsync(int, ApplyPostingJournalModel, CancellationToken)` to `/OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces/IAccountingGateway.cs` and implemented it in `/OSDevGrp.OSIntranet.Bff.ServiceGateways/AccountingGateway.cs`. It calls the generated `PostinglinesAsync` and translates exceptions with `ToServiceGatewayException()`.
- Added `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/ApplyPostingJournal/ApplyPostingJournalRequest.cs` (null-guarded `formatProvider`) and `ApplyPostingJournalFeature.cs`. The feature gets the journal, applies it, keeps lines that were not posted, clones and saves the remainder, then converts and publishes each warning.
- Added `StaticTextKey.ApplyPostingJournal` to the static-text specs in `PostingJournalFeature.cs` and `AccountingFeature.cs`, and to their `ExecuteAsyncTests` `TestCase` lists.
- Added tests:
  - `/OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/ApplyPostingJournalAsyncTests.cs`: 6 unit tests and 1 `IntegrationTest`.
  - `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Commands/Accounting/ApplyPostingJournal/ApplyPostingJournalFeature/ExecuteAsyncTests.cs`: 14 tests.
  - `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Commands/Accounting/ApplyPostingJournal/ApplyPostingJournalRequest/ConstructorTests.cs`: 2 tests.
- Refactored `SortAndClonePostingJournal` in `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs` to delegate to a new shared helper.

### Why

The PRD's acceptance criteria 7–11, 16, 29–31, and part of 33 cover the gateway and the apply command. The gateway is needed before any endpoint, and the command is the core logic. Keeping the HTTP contract out of this iteration keeps the change reviewable.

### What worked

- Gateway unit tests: 79 passed in one run, then 79 again after the changes.
- Feature tests, request tests, and gateway tests passed together: 19 DomainServices, 79 gateway.
- The full solution build finished with 0 errors and 0 warnings.
- The full `Category=UnitTest` run passed across all test projects, with 0 failures (BusinessLogic alone has 6533 tests).
- The shared helper refactor left the existing add, modify, and delete tests green.

### What didn't work

- My first gateway signature took a line collection. PRD criterion 7 requires `ApplyPostingJournalModel`, so I corrected the interface and implementation.
- An early feature test called `Assert.ThrowsAsync(...)` without `await`. The assertion was never awaited, so the test could pass without checking anything. I changed it to `await Assert.ThrowsAsync<InvalidOperationException>(...)`.
- Creating a request test file in a folder that did not exist returned `Parent directory does not exist`. I created the folder with `mkdir -p` and then wrote the file.
- I included an `IHumanVerifiableRequest` interface in the apply request by mistake. Apply does not need human verification, so I removed it.

### What I learned

- The generated `ApplyPostingJournalResultModel`, `PostingLineModel`, and `PostingWarningModel` have get-only properties and constructors with required parameters. `Build<>` does not work for them, so tests construct them directly.
- `AddFeatures` discovers features by reflection, so no DI registration was needed for the new feature (`/OSDevGrp.OSIntranet.Bff.DomainServices/ServiceCollectionExtensions.cs`).
- Feature test namespaces that contain the feature's own name (for example `ApplyPostingJournalFeature`) collide with the type name. The tests use aliases, the same way the existing delete tests do.

### What was tricky

- The PRD says the apply result's posted lines are the ones removed from the draft. The feature relies on that, and I did not re-check it against the Web API implementation in this step.
- The feature keeps lines whose identifier is not in the result, and it keeps null-identifier lines. That choice is covered by tests but not confirmed by the user.

### What warrants review

- `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/ApplyPostingJournal/ApplyPostingJournalFeature.cs`: the order of get, apply, retain, clone, save, and publish. Check that a publish failure does not undo a save.
- `/OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs`: the refactor to the shared helper.
- The empty-draft default: an empty draft goes straight to the Web API with no BFF validation. This is not yet confirmed.

### Future work

- Iteration B: HTTP endpoint and contract, built on this feature.
- Update the TODO and PRD checkboxes for the completed items. This is not done yet.

## Step 2: Moving the cloner and testing it

**Author:** main

### Prompt Context

**Verbatim prompt:** Now move the @PostingJournalCloner.cs to the folder Logic/Accounting and update the namespace. I also need you to test the metod #sym:SortAndClonePostingJournal in the test class name SortAndClonePostingJournalTests which should be placed in the folder Logic/Accounting/PostingJournalCloner at the test project

**Interpretation:** Move `PostingJournalCloner` from the command feature folder into `Logic/Accounting`, following the repo's `Logic/<Area>` layout, then add unit tests for `SortAndClonePostingJournal`.

**Inferred intent:** Put the shared helper where the rest of the shared logic lives, and cover its sorting and null-guard behavior with tests before the feature work depends on it.

### What I did

- Created `/OSDevGrp.OSIntranet.Bff.DomainServices/Logic/Accounting/PostingJournalCloner.cs` with namespace `OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Accounting`. Deleted the old file under `Features/Commands/Accounting/`.
- Updated the `using` in `ApplyPostingJournalFeature.cs` and `PostingLineFeatureBase.cs`. The add, modify, and delete features reach the helper through the base class, so they needed no changes.
- Created `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/Accounting/PostingJournalCloner/SortAndClonePostingJournalTests.cs` with 8 tests, each checking one behavior.

### Why

The repo keeps shared helpers under `Logic/<Area>` (for example `Logic/Validation`), and the tests mirror that layout. The cloner is shared across add, modify, delete, and apply, so it belongs in `Logic`.

### What worked

- The DomainServices unit suite passed after the move: 2275 tests, 0 failures, 8 of them new.

### What didn't work

- The first `create` of the moved class failed with `Parent directory does not exist`, because `Logic/Accounting` did not exist. The old file had already been removed by an `rm` in the same batch, so I created the folder with `mkdir -p` and rewrote the class from the content I had viewed.
- The first test build failed with `CS0234: The type or namespace name 'SortAndClonePostingJournal' does not exist in the namespace 'OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Accounting.PostingJournalCloner'`. The test namespace ends in `PostingJournalCloner`, so that segment shadowed the class. Renaming the alias to `PostingJournalCloner` did not help, so I renamed it to `Cloner`.

### What I learned

- Inside a test namespace whose last segment matches a class name, the namespace segment wins over a `using` alias of the same name. Pick an alias that is not a namespace segment.

### What was tricky

- The cloner has no dependencies, so the tests have no mocks and no `CreateSut`. The tests use small helpers to build inputs. This is a deliberate exception to the `CreateSut` rule you set, because there is nothing to mock.
- The generated models do not override `Equals`, so `Is.EqualTo` on the line collections compares by reference. The tests rely on that and do not need `Using(ReferenceEquals)`.

### What warrants review

- `/OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/Accounting/PostingJournalCloner/SortAndClonePostingJournalTests.cs`: the sort-order and null-sort-order cases.
- Confirm that the add, modify, and delete feature tests still pass. They do, but only in the DomainServices run.

### Future work

- None specific to this step.

## Step 3: Running and ignoring the apply integration test

**Author:** main

### Prompt Context

**Verbatim prompt:** Try to run it once more

Followed by: "Lets make it as ignored"

**Interpretation:** Run `ApplyPostingJournalAsync_WhenCalledWithDebitAndEqualCredit_ExpectNoErrors` again. If it passes, mark it `[Ignore]` as you described earlier.

**Inferred intent:** Confirm the apply path works end to end against the real services, then stop it from running in normal test runs.

### What I did

- Ran the test filtered on its full name in `/OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/`. The result was 1 passed, 0 failed, in 33 seconds.
- Added `[Ignore("Integration test verified working; ignored because it requires live MySQL and OAuth/Graph services.")]` to the test in `/OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/ApplyPostingJournalAsyncTests.cs`.
- Re-ran the gateway's `ApplyPostingJournalAsync` unit tests: 7 passed.

### Why

Your rule was to keep the integration test active until it had run successfully. It has now run, so it can be ignored.

### What worked

- The integration test passed on the second attempt with the live services, which confirms the gateway's apply call works end to end.

### What didn't work

- In the earlier session, I did not run the integration test. I had tagged it `IntegrationTest` and filtered every run to `Category=UnitTest`, so I could not claim it was verified. The first run I made in this step reported a pass, and no error came back.

### What I learned

- Integration tests here need MySQL and OAuth/Graph services running, which you can reach from this machine. Earlier I assumed they were not reachable without checking.

### What was tricky

- The `Ignore` reason text doubles as the only record of why the test is skipped. Keep it accurate if the environment changes.

### What warrants review

- `/OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/ApplyPostingJournalAsyncTests.cs`: the `[Ignore]` attribute on the integration test.

### Future work

- If the apply behavior changes, un-ignore this test and run it again against the live services.
- Update the TODO and PRD checkboxes, which are still open.
