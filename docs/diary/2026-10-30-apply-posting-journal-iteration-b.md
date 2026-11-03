# Diary: Apply posting journal, Iteration B (BFF HTTP contract and endpoint)

Iteration B of the "Apply posting journal with warning notifications" feature in `tasks/2026-10-28-apply-posting-journal/PRD.md`. It covers the BFF WebApi only: the `NotificationDto` contract, the `ApplyPostingJournalResponseDto`, and the `POST {accountingNumber}/postingjournal/apply` endpoint. The apply command and notifications query from earlier iterations are used as they are. React is not changed. The previous iteration is in `docs/diary/2026-10-30-apply-posting-journal-iteration-a.md`.

## Step 1: Notification DTO, apply endpoint, and tests

**Author:** main

### Prompt Context

**Verbatim prompt:** /suggest-next-iteration

Followed by: "Lets plan this iteration"

Followed by: "The plan is confirmet and go ahead with the implementation"

**Interpretation:** Pick the next slice from the proposals (the user chose Iteration B, the BFF HTTP contract and endpoint), then implement it.

**Inferred intent:** Expose the apply operation over HTTP with notifications in the response, so the React iterations can build on a stable contract.

### What I did

- Added `NotificationFormatMinLength`, `NotificationFormatRegexPattern` (`^Markdown$`), `NotificationTypeMinLength`, `NotificationTypeRegexPattern` (`^(Information|Warning|Error)$`), `NotificationHeaderMinLength`, and `NotificationContentMinLength` to `/OSDevGrp.OSIntranet.Bff.WebApi/Shared/ValidationValues.cs`.
- Added `/OSDevGrp.OSIntranet.Bff.WebApi/Shared/Dtos/NotificationDto.cs` with `Format`, `Type`, `Header`, and `Content`. Each property is `[Required]` and `[MinLength]`, and `Format` and `Type` also have `[RegularExpression]`. The internal static `Map(INotification)` maps the enums with `ToString()`.
- Added `/OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/ApplyPostingJournalResponseDto.cs`, which inherits `AccountingIdentificationDto` and has `Notifications`. The internal `Map(int, NotificationsResponse)` builds it.
- Added `ApplyPostingJournalAsync` to `/OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs`. It is `[Authorize(Policy = Policies.AccountingModifier)]` and `[ValidateAntiForgeryToken]`, and it has no verification headers. The flow runs the apply command, then queries notifications, then returns `Ok(ApplyPostingJournalResponseDto.Map(...))`.
- Added tests:
  - `/OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/ApplyPostingJournalAsyncTests.cs`: 13 tests.
- Added `CreateNotification` and `CreateNotificationMock` to `/OSDevGrp.OSIntranet.Bff.WebApi.Tests/Shared/Dtos/FixtureExtensions.cs`, following the two-method fixture pattern. The controller test uses this helper, so it has no local copy.

### Why

The PRD's acceptance criteria 17–21 and the WebApi part of 33 need an HTTP endpoint that returns notifications. The contract has to exist before the React iterations can consume it. The endpoint reuses the Iteration A command and the existing notifications query, so the change stays small and can be reviewed in one sitting.

### Decisions

- An empty draft is not checked in the BFF. The Web API decides, which was the approved default.
- The `Type` and `Format` checks use `[RegularExpression]`, which matches the enum names exactly.
- The apply endpoint does not use the delete endpoint's verification headers. The PRD does not ask for them.

### What worked

- `dotnet test` on `OSDevGrp.OSIntranet.Bff.WebApi.Tests` with `--filter "Category=UnitTest"`: 738 passed, 0 failed, both before and after moving the notification helper into `FixtureExtensions`.
- `Bff.WebApi` builds with 0 errors and 0 warnings.

### Known gaps

- Failure surfacing (ProblemDetails for a failed command or query) has no dedicated test. It relies on the existing error-handling filter. The matching TODO item stays unticked.
- The generated OpenAPI output has not been checked.
- Iteration C (React apply flow) and Iteration D (React notification rendering) are not started.
