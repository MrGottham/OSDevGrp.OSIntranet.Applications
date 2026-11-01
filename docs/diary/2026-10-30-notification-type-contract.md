# Diary: Notification type contract

This work records the notification-type addition to the posting-journal notification contract and the follow-up refinements to its naming, Markdown format, and React iteration scope.

## Step 1: Add notification type classification

**Author:** main

### Prompt Context

**Verbatim prompt:** I relooked at @PRD.md and @TODO.md and figure out we missed a business requirement saying that #sym:INotification should have a NotificationType which the following values Information, Warning, Error. This notification type should be used when rendering the Toast in the react application. When making #sym:MarkdownNotification the NotificationFormat should be injected in the constructor. In #sym:PostingWarningModelToNotificationConverter all markdown notifications should be Warnings. Lets updated @TODO.md and @PRD.md with this information and implement the requirements

**Interpretation:** Add a notification classification enum to the BFF contract, set posting-warning notifications to Warning, and update the PRD/TODO and toast-related requirements.

**Inferred intent:** Make notification severity explicit and preserve it across the notification contract so presentation can use it in a later UI flow.

### What I did

Added `NotificationType` with `Information`, `Warning`, and `Error`; added the corresponding notification member; updated `MarkdownNotification` and the posting-warning converter; adjusted notification tests and documented the contract in `/TODO.md` and `/tasks/2026-10-28-apply-posting-journal/PRD.md`.

### Why

The notification type distinguishes information, warnings, and errors without inferring severity from content or format.

### What worked

The DomainServices unit suite passed: 2,276 tests. The React lint and production build also completed; lint reported two existing Fast Refresh warnings, and the build reported a chunk-size warning.

### What didn't work

No implementation or test failures occurred in this step.

### What I learned

`NotificationFormat` and notification severity are separate contract dimensions: the posting warning remains Markdown while its type is Warning.

### What was tricky

The first interpretation coupled format to construction and treated the existing journal Toast as the UI integration point. Follow-up prompts refined both decisions.

### What warrants review

Review the `INotification` contract and converter output, especially that all posting-warning notifications are Warning regardless of their Markdown format.

### Future work

React notification handoff, retention, toast rendering, and dismissal are deferred to a later iteration.

## Step 2: Name the classification property `Type`

**Author:** main

### Prompt Context

**Verbatim prompt:** We should properly name the #sym:NotificationType property Type as we named the #sym:NotificationFormat in #sym:INotification

**Interpretation:** Keep the enum named `NotificationType`, but name the interface and implementation property `Type`, matching the existing `Format` property.

**Inferred intent:** Keep the model property names concise and consistent while retaining explicit enum names.

### What I did

Renamed the member on `INotification` and `MarkdownNotification` to `Type`, updated the converter test and React toast state usage, and aligned the planned DTO and JSON property names in the documentation.

### Why

The property name identifies the value's role in the notification object, while the enum retains its descriptive type name.

### What worked

The focused converter tests passed (9/9), and React lint completed with no errors.

### What didn't work

A combined shell check was blocked because its quoted search pattern contained shell-special backticks. The diagnostic was: `Command blocked: contains dangerous shell expansion patterns (e.g., parameter transformation, indirect expansion, or nested command substitution) that could enable arbitrary code execution. Please rewrite the command without these expansion patterns.` The formatting check and a rewritten search ran successfully afterward.

### What I learned

The HTTP/React contract is still a planned surface, so documentation should distinguish its future `type` property from the BFF enum name `NotificationType`.

### What was tricky

The selected React Toast was an existing journal-operation toast and was not yet rendering BFF notifications; changing it would have implied notification UI work before that integration exists.

### What warrants review

Verify future DTO mapping uses `INotification.Type` and serializes the intended `type` field when that layer is implemented.

### Future work

No additional BFF changes are implied by this property rename.

## Step 3: Keep Markdown format intrinsic to `MarkdownNotification`

**Author:** main

### Prompt Context

**Verbatim prompt:** At #sym:MarkdownNotification we should not provide the format but let the Format property tell about the Markdown format

**Interpretation:** Remove `NotificationFormat` from the constructor and have `Format` always return `NotificationFormat.Markdown`.

**Inferred intent:** A class specifically named `MarkdownNotification` should not accept a contradictory format value.

### What I did

Changed the constructor to accept only `NotificationType`, header, and content; made `Format` return the Markdown enum value; updated all constructor calls and the PRD/TODO requirements.

### Why

The class itself defines the notification format, so it should not permit callers to supply an inconsistent one.

### What worked

The complete DomainServices unit suite passed: 2,276 tests. `git diff --check` was clean, and the obsolete format-injection search returned no matches.

### What didn't work

No code or test failure occurred in this step.

### What I learned

Only the notification type varies for this concrete notification implementation; its format is fixed by the class.

### What was tricky

Every test fixture and converter call constructing `MarkdownNotification` needed the same signature update, while the interface format property remained unchanged.

### What warrants review

Confirm `Format` remains `NotificationFormat.Markdown` and the converter continues constructing warnings with `NotificationType.Warning`.

### Future work

Add a distinct notification implementation only if another format is introduced.

## Step 4: Defer React notification presentation

**Author:** main

### Prompt Context

**Verbatim prompt:** We have not implementing anything for notification in the react application yet as this gona be implemented in a later iteration

**Interpretation:** Do not change the React application's Toast behavior now; retain notification display requirements as future work and make the deferral explicit in the planning documents.

**Inferred intent:** Keep this iteration focused on the BFF notification contract and avoid conflating existing journal error toasts with future server-notification rendering.

### What I did

Reverted the toast styling/state changes made in `osdevgrp.osintranet.react/src/components/PostingJournal.jsx`. Marked React notification handoff, retention, rendering, and dismissal as later-iteration work in `/TODO.md` and `/tasks/2026-10-28-apply-posting-journal/PRD.md`, while retaining the current apply-flow and accounting-refresh requirements.

### Why

The React notification component and its integration with server notifications are not part of this iteration.

### What worked

React lint passed with zero errors and two existing context Fast Refresh warnings. `git diff --check` passed.

### What didn't work

No code or validation failure occurred in this step.

### What I learned

The BFF contract can carry notification severity before the React application consumes it; planning documents need to make that sequencing explicit.

### What was tricky

The existing Toast in `PostingJournal.jsx` presents local validation/operation errors and is not the future notification renderer. It needed to remain unchanged.

### What warrants review

Check that future React work consumes the notification `type` field and maps `Information`, `Warning`, and `Error` to the intended toast styles.

### Future work

Implement React notification receipt, refresh-safe retention, Markdown rendering, individual dismissal, and manual acceptance testing in a later iteration.
