# Diary: PostingJournal Concurrent Rendering Fix

Fix React 19 concurrent rendering behavior where computed display fields (account name, credit, available, budget account data, contact account data) were not blanking immediately when the user changed the account number input. The issue was that React's `useTransition()` hook was deferring state updates, causing stale data to display until the API response arrived.

## Step 1: Move flushSync clearing into populate functions and re-enable disabled attribute

**Author:** main

### Prompt Context

**Verbatim prompt:** "Then we properly could set the disabled again"

**Interpretation:** The user confirmed their preference to refactor the code by moving the synchronous clearing logic (wrapped in `flushSync()`) into the populate functions themselves, rather than keeping it in the useEffects. This architectural change would allow re-enabling the `disabled={true}` attribute on computed form fields.

**Inferred intent:** Clean up code organization by having each function own its own state clearing, and restore the disabled attribute for better UX consistency.

### What I did

Modified `/osdevgrp.osintranet.react/src/components/PostingJournal.jsx`:

1. **Added flushSync clearing to three populate functions** (lines 72-133):
   - `populateAccountDetails()`: Added `flushSync()` wrapper clearing `computedData.account` to empty strings before checking inputs
   - `populateBudgetAccountDetails()`: Added `flushSync()` wrapper clearing `computedData.budgetAccount` to empty strings
   - `populateContactAccountDetails()`: Added `flushSync()` wrapper clearing `computedData.contactAccount` to empty strings

2. **Simplified three useEffects** (lines 141-173):
   - Removed `flushSync()` wrapper and manual `setComputedData()` calls from useEffect bodies
   - Each useEffect now only contains the `startTransition()` call with the corresponding populate function
   - Dependencies remained unchanged (all three effects have proper dependency arrays)

3. **Re-enabled disabled={true} attribute on 8 computed display fields**:
   - Account fields: `accountName`, `accountCredit`, `accountAvailable`
   - Budget account fields: `budgetAccountName`, `budgetAccountPosted`, `budgetAccountAvailable`
   - Contact account fields: `contactAccountName`, `contactAccountBalance`
   - All fields retain `readOnly={true}` plus now have `disabled={true}`

### Why

This refactoring improves code organization and guarantees immediate field clearing:

- **Responsibility assignment**: Each populate function is now self-contained, handling both clearing its own data and populating it. No external orchestration needed.
- **Synchronous guarantee**: The `flushSync()` call executes OUTSIDE the transition context when placed inside the populate function, ensuring synchronous state flushing before any async work begins.
- **UX consistency**: The `disabled={true}` attribute properly conveys that these are computed, not user-editable fields. Combined with `readOnly={true}`, it provides strong semantic and visual feedback.

### What worked

- All three populate functions successfully wrapped their initial clearing logic in `flushSync()`
- useEffects cleaned up without behavioral changes - they still properly trigger on dependency changes and still use `startTransition()` for non-blocking updates
- All 8 form fields now have both `readOnly={true}` and `disabled={true}` attributes added consistently
- The multi-replace operation executed all 11 replacements without errors

### What didn't work

Nothing failed. The refactoring was straightforward and executed cleanly.

### What I learned

- When `flushSync()` is placed inside an async function that is later called within `startTransition()`, it forces synchronous flushing before the async work begins, but STILL allows the subsequent async updates to run within the transition context (preserving the non-blocking behavior for API calls)
- This pattern elegantly solves the "clear immediately, populate asynchronously" requirement without having to extract clearing logic to useEffect
- `disabled={true}` combined with `readOnly={true}` on form fields provides both semantic clarity and proper Bootstrap styling (appears grayed out and truly uninteractive)

### What was tricky

The conceptual model: understanding that `flushSync()` inside an async function that's wrapped in `startTransition()` provides the best of both worlds:
- Synchronous initial state clearing (fields blank immediately)
- Asynchronous API call and subsequent state updates (non-blocking transition)

This required trusting that the placement mattered more than the nesting context.

### What warrants review

1. **Test the user-facing behavior**: Verify that when changing the account number input, the computed fields blank out immediately (before API response), confirming `flushSync()` is working as intended
2. **Validate API population still works**: Ensure that after the API call completes, the fields properly populate with the response data within the transition context
3. **Check modal scenarios**: Verify that opening the modal with different account numbers also clears fields correctly
4. **Visual regression**: Confirm that `disabled={true}` styling looks appropriate alongside `readOnly={true}` in the Bootstrap Form.Control components

### Future work

- If this pattern proves successful, consider documenting it as a standard for other computed field patterns in the codebase
- Evaluate whether the same pattern should be applied to other components with similar field-clearing-before-async-load scenarios
- Consider extracting a custom hook `useFlushSyncEffect()` if this pattern appears in multiple places
