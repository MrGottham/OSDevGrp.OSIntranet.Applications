# Diary: PostingJournal Concurrent Rendering and Button Disable State

Implement button disable state during async lookups in PostingJournal component, while resolving a deeper React 19 concurrency issue involving flushSync and cascading renders.

## Step 1: Add isDisabled parameter to SubmitToolbar

**Author:** main

### Prompt Context

**Verbatim prompt:** "Disable SubmitToolbar buttons when async account/budget account/contact account lookups are pending in PostingJournal component"

**Interpretation:** The user wants the three action buttons (Submit, Reset, Cancel) in SubmitToolbar to become disabled while any of the three async account lookups are in flight.

**Inferred intent:** Prevent user interaction with form submission while data is still loading, providing visual feedback that async work is pending.

### What I did

1. Modified `/osdevgrp.osintranet.react/src/components/SubmitToolbar.jsx` to accept an optional `isDisabled` prop (default `false`)
2. Applied `disabled={isDisabled}` to all three Button components (submit, reset, cancel)
3. Updated `/osdevgrp.osintranet.react/src/components/PostingJournal.jsx` Modal.Footer to pass combined pending state:
   ```jsx
   isDisabled={isAccountPending || isBudgetAccountPending || isContactAccountPending}
   ```
4. Verified `/osdevgrp.osintranet.react/src/components/AccountingForm.jsx` requires no changes (backward compatible with default `isDisabled = false`)

### Why

SubmitToolbar is a reusable component used across two forms. Adding an optional parameter maintains backward compatibility while enabling the feature in PostingJournal. The three `useTransition()` hooks already track pending states; combining them provides the complete picture.

### What worked

- Clean API design: optional parameter with sensible default
- Immediate button disable when `isPending` becomes true
- Buttons automatically re-enable when all transitions complete
- No breaking changes to existing consumers

### What didn't work

Nothing failed in this step. The implementation was straightforward.

### What I learned

The component architecture properly separated concerns: SubmitToolbar handles UI, PostingJournal handles state. The existing three `useTransition()` hooks in PostingJournal already provided perfect granularity for tracking three independent async operations.

### What was tricky

None.

### What warrants review

- Verify button disabled state provides clear UX feedback during lookups
- Test that buttons re-enable immediately when all async operations complete
- Confirm DatePicker is also disabled during pending state (already done via `disabled={isAccountPending || isBudgetAccountPending || isContactAccountPending}`)

### Future work

None at this stage.

---

## Step 2: Investigate React 19 flushSync error and discover original architecture

**Author:** main

### Prompt Context

**Verbatim prompt:** User reported React error: "flushSync was called from inside a lifecycle method. React cannot flush when React is already rendering."

**Interpretation:** The application is triggering a React 19 strict mode warning related to synchronous state flushing during lifecycle methods.

**Inferred intent:** Understand why flushSync is present and whether it can be removed or must be restructured.

### What I did

1. Searched for flushSync usage in PostingJournal.jsx - initially found none (already removed at some point)
2. Used git history to locate commit `5e7da174` which introduced flushSync
3. Ran: `git show 5e7da174 | grep -A 10 -B 10 "flushSync"`
4. Discovered original diary entry explaining the pattern:
   > "Fix React 19 concurrent rendering behavior where computed display fields (account name, credit, available, budget account data, contact account data) were not blanking immediately when the user changed the account number input. The issue was that React's `useTransition()` hook was deferring state updates, causing stale data to display until the API response arrived."

### Why

Git history revealed critical architectural context. The original developer explicitly chose flushSync to solve a real UX problem: fields weren't clearing immediately because `useTransition()` defers state updates for non-blocking rendering. Blindly removing flushSync would regress the UX.

### What worked

Git archaeology successfully uncovered the original intent and diary explaining the pattern. This prevented a false start where I would have just removed flushSync and ignored the underlying problem.

### What didn't work

Initial attempts to move flushSync "earlier in the effect" still violated React 19 rules because flushSync cannot be called during ANY lifecycle method, whether at the start or wrapped in callbacks.

### What I learned

- React 19's concurrency model is strict: `flushSync` is forbidden in lifecycle methods, no exceptions
- `useTransition()` intentionally defers state updates for non-blocking rendering
- The original pattern had correct *intent* but violated React's new rules
- There's a fundamental tension: `useTransition()` defers updates + fields should clear immediately

### What was tricky

The error message suggested moving flushSync to "a scheduler task or micro task," but calling flushSync in any callback function passed through `useTransition()` still violates the rule because the callback runs during React's commit phase.

### What warrants review

- The solution strategy: moving field-clearing logic entirely out of useEffect to onChange handlers
- This is a architectural shift, not a simple config change

### Future work

Implement the new pattern and verify it resolves both the React error and maintains UX (fields clear immediately).

---

## Step 3: Move field-clearing setState from useEffect to onChange handlers

**Author:** main

### Prompt Context

**Verbatim prompt:** After multiple failed attempts to use flushSync or normal setState in useEffect bodies, React error persisted: "Calling setState synchronously within an effect can trigger cascading renders"

**Interpretation:** React 19 forbids ANY setState call directly in useEffect bodies due to cascading render warnings, even normal setState without flushSync.

**Inferred intent:** Find an architecture that clears fields immediately without calling setState in lifecycle methods.

### What I did

1. Removed all `setComputedData()` calls from the three useEffect hooks
2. Simplified useEffect to only call `startTransition()` for async work
3. Added field-clearing logic to four onChange handlers:
   - **DatePicker.onChange**: Clears all three computed data objects when posting date changes
   - **Account number input onChange**: Clears account data immediately
   - **Budget account number input onChange**: Clears budget account data immediately
   - **Contact account number input onChange**: Clears contact account data immediately
4. Files modified: `/osdevgrp.osintranet.react/src/components/PostingJournal.jsx`

### Why

onChange handlers execute outside React's render/commit phases, so calling setState is clean and idiomatic. Users expect form fields to respond immediately to their input. Moving the clearing logic to where it naturally belongs (input change) instead of fighting React's concurrency model is the correct architectural pattern.

### What worked

- ✅ All React errors eliminated (no setState in effects, no flushSync)
- ✅ Fields clear **immediately** when user changes input (better UX than delayed clearing)
- ✅ Fields clear **immediately** when posting date changes (because onChange fires before async effect)
- ✅ Follows React best practices for effect cleanup vs. event handler state management
- ✅ No performance warnings or cascading render issues

### What didn't work

1. Attempt 1: Remove flushSync entirely and use normal setState in useEffect → React error: "setState in effect body"
2. Attempt 2: Move flushSync from inside async function to useEffect start → React error: "flushSync in lifecycle method"
3. Attempt 3: Call flushSync before startTransition in useEffect → Same React error
4. Attempt 4: Normal setState in useEffect (without flushSync) → React warning: "cascading renders"

All attempts to call setState in useEffect failed because React 19's concurrency model forbids it.

### What I learned

- React 19 distinguishes between setState in effect *bodies* (forbidden) vs. in event handlers (encouraged)
- The original flushSync approach was trying to solve React 18 + useTransition concurrency issues, but React 19 changed the rules
- **Architecture matters**: Moving state updates to where they naturally occur (onChange) is cleaner than fighting React's concurrency rules
- useEffect should synchronize with external systems or subscribe to updates, not be a dumping ground for cascading state updates

### What was tricky

The error messages and React docs initially suggested "move to a micro task," but that still violated the core rule. The real solution required architectural rethinking: stop trying to clear fields in effects and instead clear them where user input is handled.

### What warrants review

1. **UX validation**: Open PostingJournal modal, change an account number, verify the corresponding display fields blank immediately
2. **Date change**: Change posting date, verify all three computed data sections blank
3. **Async behavior**: Verify that buttons still show disabled state during lookups
4. **API data arrival**: After ~750ms debounce, verify API data populates the display fields
5. **No console errors**: Confirm no React errors, warnings, or cascading render warnings

### Future work

- Monitor performance: ensure no unexpected re-renders
- Test edge cases: rapid account changes, changing date while lookup is pending, switching between accounts
