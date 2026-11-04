# Diary: Apply posting journal, Iteration 2 (notification toasts)

Iteration 2 of the "Apply posting journal with warning notifications" feature in `tasks/2026-10-28-apply-posting-journal/PRD.md`. It shows the notifications returned by apply as toasts in the React accounting view, with Markdown rendering. The previous iterations are in `docs/diary/2026-10-30-apply-posting-journal-iteration-a.md`, `-b.md`, and `-c.md`.

## Step 1: Notification toasts after apply

**Author:** main

### Prompt Context

**Verbatim prompt:** /suggest-next-iteration

Followed by: "Let make a plan for iteration 2"

Followed by: "As we have #sym:uuid as a dependency I think we shold update the plan to use than instead of crypto. Are we planing to reuse posting-journal__toast?"

Followed by: "Perfect make sure the plan is updated", then "Now implement the plan".

**Interpretation:** Build iteration 2 from the proposed options: show the notifications from the apply response as toasts, using Markdown rendering through `react-markdown`.

**Inferred intent:** Users see warnings and errors from the posting journal apply, and can read formatted content and dismiss it.

### What I did

- Added `react-markdown` (`^10.1.0`) to `osdevgrp.osintranet.react/package.json`. `npm audit` reports 0 vulnerabilities.
- Created `osdevgrp.osintranet.react/src/components/Notifications.jsx`:
  - A top-end `ToastContainer`, matching the posting-journal toasts, one `Toast` per notification, with `autohide={false}`.
  - `type` maps to the Bootstrap background: `Information` to `info`, `Warning` to `warning`, `Error` to `danger`.
  - `format === 'Markdown'` renders the content with `<Markdown>`. Other formats render as plain text.
- Updated `PostingJournal.jsx` so `applyPostingJournal` passes `response.notifications` to `onPostingJournalApplied`.
- Updated `Accounting.jsx`:
  - Notifications are held in `{ accountingNumber, items }` state. They are shown only when the stored accounting number matches the route parameter. That avoids calling `setState` in an effect.
  - `handlePostingJournalApplied(notifications)` appends the notifications with `uuid` `v7` ids after the reload succeeds. On a reload error it calls `showBoundary` and appends nothing.
  - `handleNotificationDismissed(id)` removes one toast.
- Added `notifications__*` classes to `App.css`. They reuse the width breakpoints of the posting-journal toasts, but are separate classes so the two components do not depend on each other.

### Why

The apply response carries notifications that the user needs to see. The React side already reloads the accounting after apply (iteration C), so the notifications are handed off across that reload. Keeping them in `Accounting` means they survive the reload but are not persisted.

### Decisions

- Toasts are top-end, the same as the posting-journal toasts, so both kinds appear in the same place. The container was first set to bottom-end. The user saw it at the bottom of the screen and asked for the top, so I changed it.
- `autohide={false}`: warnings and errors stay until the user dismisses them.
- The posting-journal toast classes are not reused. They are scoped to that component's toast container.

### What didn't work

- Nothing failed in this step.

### What I learned

- The `Accounting` component stays mounted during the reload, because the `Loading` branch replaces only its content. State held in `Accounting` therefore survives the reload.

### What worked

- Passing the notifications through the existing `onPostingJournalApplied` callback needed no change to the service layer. The API response already carried them.

### What was tricky

- Keeping the notifications across the reload but clearing them on accounting switch. The route has no `key`, so the component is not remounted between accountings. This was revisited in Step 2.

### What warrants review

- `Accounting.jsx`: `handlePostingJournalApplied` and the notification state handling.
- `Notifications.jsx`: Markdown rendering. `react-markdown` v10 does not render raw HTML by default, so the content is not trusted as HTML.

### Verification

- `npm run lint`: only the 2 pre-existing warnings in `contexts/HelperContext.jsx` and `contexts/ServiceContext.jsx`.
- `npm run build`: passes with a chunk-size warning. The `App` chunk is about 717 kB minified. I did not measure the size before this change, so I can't say how much `react-markdown` added.
- Not verified: the toasts in a running browser against the full stack (MySQL, OAuth). PRD criteria 24 to 28 still need a manual check.

### Future work

- Manual browser check of criteria 24 to 28, including that raw HTML in Markdown content is not rendered.

## Step 2: Hold notifications in a context-named state without the accounting number

**Author:** main

### Prompt Context

**Verbatim prompt:** Looking at @Accounting.jsx I think we should remane the #sym:notificationState to notificationContext and remove the accountingNumber from the state so the state only contains the notification items

Followed by: "Why did you make the function #sym:AccountingContent ?"

Followed by: "Could the #sym:handleNotificationDismissed be moved into the @Notifications.jsx component?"

**Interpretation:** Rename the notification state, drop the accounting number from it, and keep the notifications correct when the accounting changes.

**Inferred intent:** A simpler state shape with the same behaviour: toasts clear when the user switches accounting, and are not shown for another accounting.

### What I did

- Renamed the state to `notificationContext` in `osdevgrp.osintranet.react/src/components/Accounting.jsx`. It is a plain array of items, initialised to `[]`.
- Removed the `accountingNumber` filter at render time. `Notifications` receives `notificationContext` directly.
- Added a thin `Accounting` wrapper that renders `AccountingContent` with `key={accountingNumber}`. The previous body is now `AccountingContent`.
- Left `handleNotificationDismissed` in `Accounting`. The question of moving it into `Notifications.jsx` was discussed and not changed.

### Why

Removing the accounting number from the state removed the only thing that cleared the toasts when the accounting changed. The route in `Main.jsx` has no `key`, so `Accounting` stays mounted across accounting changes.

### What worked

- Remounting with `key={accountingNumber}` clears the state without any effect, and keeps the number out of the notification state.

### What didn't work

- The first attempt cleared the state in an effect on `accountingNumber`. Lint rejected it:

  ```
  react-hooks/set-state-in-effect  Avoid calling setState() directly within an effect
  ```

### What I learned

- `react-hooks` v7 in this repo flags any synchronous `setState` in an effect, including a reset on a prop change.

### What was tricky

- The wrapper remounts the whole accounting view on every accounting change, not only the toasts. This is the trade-off accepted for the simpler state.

### What warrants review

- `Accounting.jsx`: the `Accounting` wrapper and `AccountingContent`. Check that switching accounting shows no stale toasts.

### Future work

- If `Notifications` should own its list, move the state and both handlers into it. Left undone because the state is currently shared with `handlePostingJournalApplied`.

### Verification

- `npm run lint`: only the 2 pre-existing warnings.
- `npm run build`: passes.

## Step 3: Combine duplicate toast CSS rules

**Author:** main

### Prompt Context

**Verbatim prompt:** Shuold @App.css be refactored to avoid duplicate code?

Followed by: "Lets do that"

**Interpretation:** Remove the duplicated `posting-journal__toast*` and `notifications__toast*` rules in `App.css`.

**Inferred intent:** One set of toast styles shared by both components, with no change to the rendered output.

### What I did

- In `osdevgrp.osintranet.react/src/App.css`, combined each rule's selectors so that `.posting-journal__*` and `.notifications__*` share one declaration. This covers the container, toast, and header rules and the 576px and 768px media queries.
- Left the class names in `PostingJournal.jsx` and `Notifications.jsx` unchanged.

### Why

The two toast styles were identical, so keeping both sets meant editing them twice whenever the breakpoints or header style changed.

### What worked

- Combining selectors needed no change to the components.

### What didn't work

- Nothing failed in this step.

### What I learned

- The attached `file_App.css` snapshot matches the workspace file before this change, so it was a valid reference for the duplication.

### What was tricky

- Nothing tricky. The consolidated file only differs from the original by the selector lists.

### What warrants review

- `App.css`: each combined rule covers both prefixes, so a future change to one component's toast styles applies to both.

### Future work

- A shared class such as `.toast-stack` would be cleaner, but would mean changing class names in `PostingJournal.jsx`. Not done.

### Verification

- `npm run lint`: only the 2 pre-existing warnings.
- `npm run build`: passes.

## Step 4: Identifying notifications in the BFF

**Author:** main

### Prompt Context

**Verbatim prompt:** "As the react application gona need a uuid to dismiss the toast I think the best solution is to set the uuid in each notification from the BFF Domain layer. Let add a Guid Identifer getter as the first propertyto @INotification.cs and let it be initialized by the constructor @MarkdownNotification.cs. Let @PostingWarningModelToNotificationConverter.cs initialize the @MarkdownNotification.cs with a new Guid. Then we could expose the Identifier as a required Guid in the @NotificationDto.cs and let the @Accountings.jsx and @Notifications.jsx use this instead of building thier own one"

**Interpretation:** The React code should not generate its own ids. The BFF domain should assign a `Guid` to each notification, and that value should flow through the DTO to the toasts.

**Inferred intent:** A stable, server-assigned key for each toast, so dismissal does not depend on client-side state.

### What I did

- `INotification` gained `Guid Identifier { get; }` as its first property.
- `MarkdownNotification` takes `Guid identifier` as its first constructor argument and exposes `Identifier`.
- `PostingWarningModelToNotificationConverter` creates notifications with `Guid.NewGuid()`.
- `NotificationDto` gained `public required Guid Identifier { get; init; }`, mapped from `notification.Identifier`.
- Test call sites (`RetrieveAsyncTests`, `PublishAsyncTests`, `AddDomainServicesTests`, `NotificationsFeature` `ExecuteAsyncTests`) pass `Guid.NewGuid()`. `FixtureExtensions.CreateNotificationMock` sets up `Identifier`.
- In React, `Accounting.jsx` no longer generates ids. It appends the notifications as received, and dismissal filters on `notification.identifier`. The `uuid` import was removed from `Accounting.jsx`. `Notifications.jsx` uses `notification.identifier` for the key and the dismiss call.

### Why

Server-assigned identifiers mean the client does not need to generate and track its own ids. The `uuid` dependency is still used elsewhere (`PostingJournal.jsx`, `FormHelper.jsx`), so it stays in `package.json`.

### What worked

- The change compiled across the BFF projects on the first try. Both affected test projects passed all unit tests: 2,276 in DomainServices.Tests and 734 in WebApi.Tests.
- The React lint and build passed, with no new warnings.

### What didn't work

- My first pass left `[Required]` off the `Guid` property in `NotificationDto`, even though every other property on the DTO has it. The user pointed this out, and I restored it. The `required` keyword only enforces initialization in C#, while `[Required]` is what the data annotation validation sees.
- A React edit meant to remove the `uuid` import was a no-op on the first attempt. The import was still there, so I removed it in a second edit.

### What I learned

- `NotificationDto` is mapped from `INotification` and returned through `ApplyPostingJournalResponseDto`, so `identifier` reaches React in the apply response. I did not check whether the generated NSwag client in `OSDevGrp.OSIntranet.Bff.WebApi` or other consumers depend on this DTO's shape.

### What was tricky

- The request named `@Accountings.jsx`. The `uuid` usage is in `Accounting.jsx`, so I changed that file. `Accountings.jsx` does not handle notifications.
- The domain and DTO use `Guid` directly, but the React side receives the value as a string, so the frontend keys on it as a plain string.

### What warrants review

- `PostingWarningModelToNotificationConverter.cs`: the new `Guid.NewGuid()` call. A converter unit test that asserts `Identifier` is not `Guid.Empty` would be a useful addition, and I have not added one yet.
- `Accounting.jsx`: `handleNotificationDismissed` now takes an `identifier`.

### Future work

- Add the converter assertion mentioned above.
- Run the manual full-stack check of PRD criteria 24–28 (toasts, dismissal, Markdown rendering).

## Step 5: Fixing the lint warnings in the contexts

**Author:** main

### Prompt Context

**Verbatim prompt:** "Let fix this, bud does any components not use the HelperContext and ServiceContext?"

**Interpretation:** Fix the two `react-refresh/only-export-components` warnings, and check which components use the contexts.

**Inferred intent:** Lint should be clean, and I should know whether the contexts are used everywhere before changing them.

### What I did

- Five components use neither context: `AccountingCard`, `DeleteConfirmation`, `Loading`, `Notifications`, and `PostingLineCollection`. None of them needed changes.
- `HelperContext.jsx` and `ServiceContext.jsx` were replaced by `HelperContext.js` and `ServiceContext.js`. Each `.js` file has the context object and the provider. The provider uses `createElement` instead of JSX, because a `.js` file does not support JSX.
- The consumers' imports (`'../contexts/HelperContext'`) were left unchanged. Vite resolves `.js` and `.jsx`, so the import paths still work.

### Why

The react-refresh rule only checks `.jsx` files. Moving the non-component exports into `.js` files removes the warning without touching the 30 import lines across 22 components.

### What worked

- `npm run lint` now shows no warnings or errors.
- `npm run build` passes.

### What didn't work

- Nothing failed in this step.

### What I learned

- The rule checks `.jsx` files, and the warning only applies to files that export a component and a non-component together.

### What was tricky

- A `.js` file cannot contain JSX, so the provider had to be written with `createElement`.

### What warrants review

- `HelperContext.js` and `ServiceContext.js`: check that the providers still wrap the app in `App.jsx`, and that the apps load in the browser.

### Future work

- None from this step.

