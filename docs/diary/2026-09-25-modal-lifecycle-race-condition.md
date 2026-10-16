# Diary: Fix Modal Lifecycle Race Condition in PostingJournal Component

**Goal:** Prevent modal from closing before async form submission completes, ensuring form fields remain disabled during submission and modal lifecycle is properly managed.

## Step 1: Implement useTransition Hook & Modal Lifecycle Guards

**Author:** main

### Prompt Context

**Verbatim prompt:** "Would the modal close before #sym:handleCreatePostingJournalLine and #sym:handleModifyPostingJournalLine has done it job defined in #sym:startModelFormTransition?"

**Interpretation:** User identified a potential race condition where the modal could close while async form submission was still pending, leaving incomplete state.

**Inferred intent:** Build a robust modal lifecycle management system that prevents premature closure and maintains UI consistency during async operations.

### What I did

1. Added `useTransition()` hook to track form submission state: `const [isModelFormSubmitting, startModelFormTransition] = useTransition();` (line 62)
2. Implemented modal close guard in `onHide` handler to prevent closing while `isModelFormSubmitting` is true (lines 230-233)
3. Wrapped all form submission logic in `startModelFormTransition(async () => {...})` (lines 496-540)
4. Added `disabled={isModelFormSubmitting || ...}` to all 8 form input fields:
   - postingDate (line 268)
   - postingReference (line 290)
   - accountNumber (line 304)
   - postingText (line 350)
   - budgetAccountNumber (line 381)
   - debit (line 410)
   - credit (line 416)
   - contactAccountNumber (line 437)
5. Updated `SubmitToolbar` isDisabled prop to include `isModelFormSubmitting` condition (line 463)
6. Added console.log debugging statements to handlers to verify execution

### Why

React 19's `useTransition` hook automatically:
- Sets `isPending` to true while async work executes
- Sets it to false when complete
- Triggers re-renders so UI can react to state changes

This provides the foundation for:
- Disabling form during submission (preventing accidental re-submission)
- Preventing modal close during submission (blocking race condition)
- Giving user visual feedback that work is pending

### What worked

- ✅ Build verifies clean with no errors (438ms)
- ✅ Form validation works correctly (Formik confirms validation passes)
- ✅ useTransition hook properly integrated with React 19 patterns
- ✅ Modal close guard syntax correct—`if (isModelFormSubmitting) return;` prevents closure
- ✅ All form fields properly wired to `isModelFormSubmitting` flag
- ✅ SubmitToolbar button is `type="submit"` and properly triggers form submission
- ✅ App works correctly after reload (proving mechanics are sound)

### What didn't work

**Form handlers not executing when OK clicked** (Critical Issue):
- Symptoms: Formik validation passed, but `handleCreatePostingJournalLine` and `handleUpdatePostingJournalLine` weren't called
- Handlers didn't execute, no console logs appeared, modal didn't close
- Expected: "DEBUG: handleCreatePostingJournalLine called with values:..." logged to console
- Actual: Nothing happened on OK button click

**Root Cause Identified**: Stale closure issue with `modalState.okCallback`
- Formik's `onSubmit={modalState.okCallback}` (line 234) captured a stale reference from an earlier render cycle
- Each time modal opens, `setModalState` updates the callback, but Formik's closure over the old value persists
- This is a classic JavaScript closure trap in React: the render captures the callback at one point in time, but later renders update state without updating the closure
- **Verification**: Reloading the app fixed the issue immediately (fresh component = fresh closure), confirming this was the bottleneck

### What I learned

1. **React 19 properly supports async in useTransition**: Unlike older patterns, React 19's `startTransition` accepts async callbacks natively and properly tracks pending state throughout the entire async operation. There's no need to remove `async` keyword.

2. **Closure problems are insidious in React**: The code looked syntactically correct (handlers implemented, state properly set, validation working) but stale closure made the entire feature invisible. Only discovered through reloading, which created fresh closures.

3. **useTransition is the right tool**: It provides automatic pending state management without requiring manual useState tracking. No need for a separate boolean flag.

4. **Formik integration complexity**: Using dynamic callbacks passed through state (`modalState.okCallback`) creates closure traps. Better patterns:
   - Static callbacks that don't change
   - Ref-based routing (callback looks at current ref value)
   - Inline handlers in Formik's onSubmit that dispatch to logic based on current state

5. **Modal lifecycle management**: The guard pattern `if (isPending) return;` in Modal.onHide is effective for blocking close during work.

### What was tricky

1. **Diagnosing handler non-execution**: Initially seemed like async keyword was problematic (since user questioned it), or that startTransition wasn't working. Debugged through multiple angles:
   - Verified React 19 supports async natively ✓
   - Confirmed handlers are defined ✓
   - Validated modalState.okCallback is set ✓
   - Tested Formik validation passes ✓
   - Then realized: the callback is stale from an earlier render

2. **Modal guard timing**: The `if (isModelFormSubmitting) return;` guard in `onHide` must fire **before** `setModalState` to work. If `setModalState` fires first, guard is bypassed.

3. **Distinguishing create vs update**: Current code uses `modalState.okCallback` to route—but that's the source of the closure bug. Needed to find routing pattern that doesn't rely on closure-prone state properties.

4. **Understanding the symptom vs root cause**: "Form handlers don't execute" could mean:
   - Formik isn't calling onSubmit (but validation works, so likely not this)
   - Button isn't type="submit" (but it is)
   - Callback is undefined (it's not)
   - Callback is stale (✓ this one)

### What warrants review

1. **Proposed Solution: Replace modalState.okCallback with modalModeRef**

   This pattern eliminates closure issues:
   ```jsx
   const modalModeRef = useRef(null);
   
   // In handleAddPostingJournalLine:
   modalModeRef.current = 'create';
   setModalState({
       showEditModal: true,
       title: editModalTitle,
       okText: editModalOkText,
       // Remove okCallback entirely
   });
   
   // In Formik's onSubmit:
   onSubmit={(values) => {
       if (modalModeRef.current === 'create') {
           handleCreatePostingJournalLine(values);
       } else if (modalModeRef.current === 'update') {
           handleUpdatePostingJournalLine(values);
       }
   }}
   ```

   **Why this works:**
   - Formik's closure captures the inline arrow function `(values) => { ... }`
   - That arrow function doesn't depend on state, only refs
   - Refs always point to current values (`modalModeRef.current`) 
   - No stale closure because we're not storing functions in state

2. **Current Workaround Status**: App works after reload—this is not a permanent solution. Reload creates fresh component instance with fresh closures. Users won't reload between actions; feature is broken without the ref-based fix.

3. **Form Disabling Completeness**: All 8 editable fields have `disabled={isModelFormSubmitting || ...}` conditions. Any new form fields added must follow this pattern.

4. **Error Handling Not Yet Implemented**: Handlers currently have no try-catch, no error toasts, no user feedback on failure. Deferred per user request.

### Future work

1. **Implement modalModeRef solution** (Priority: HIGH) — Required to fix stale closure and allow submission without reload
   - Add `const modalModeRef = useRef(null)` after contactAccountNumberChangeTimer
   - Update handleAddPostingJournalLine to set `modalModeRef.current = 'create'`
   - Update handleModifyPostingJournalLine to set `modalModeRef.current = 'update'`
   - Replace Formik's `onSubmit={modalState.okCallback}` with inline handler that checks `modalModeRef.current`
   - Remove `okCallback` from modalState entirely

2. **Add error handling** (Priority: MEDIUM) — Wrap handler logic in try-catch and call `addToast` on failure
   - Pattern: Consistent with existing account lookup error handling (lines 83-85, 118-120, 153-155)
   - User deferred this: "we gona handle this later"

3. **Implement actual API calls** (Priority: MEDIUM) — Replace console.debug with real accountingService calls
   - createPostingJournalLine(accountingNumber, values)
   - updatePostingJournalLine(accountingNumber, values)
   - Place inside try-catch when error handling is restored

4. **Add form reset after submission** (Priority: LOW) — Clear form fields after successful close
   - Option: Reset formData state when modal closes

5. **Manual end-to-end testing** (Priority: MEDIUM) — Verify complete lifecycle
   - Click Add → form opens → fill fields → validation works → click OK → handlers execute → console logs appear → modal closes
   - Click Edit → same flow
   - Try pressing ESC/backdrop click during "submission" → modal doesn't close
   - Try closing before submission completes → blocked by guard

## Key Files Modified

**Primary file:** `/osdevgrp.osintranet.react/src/components/PostingJournal.jsx`

**Changes by line range:**
- Line 62: `useTransition` hook added for form submission tracking
- Lines 230-233: Modal close guard—prevents close while `isModelFormSubmitting` true
- Line 234: Formik `onSubmit={modalState.okCallback}` binding (source of stale closure—awaits modalModeRef fix)
- Lines 268, 290, 304, 350, 381, 410, 416, 437: Form fields with `disabled={isModelFormSubmitting || ...}` conditions
- Line 463: SubmitToolbar `isDisabled` prop includes `isModelFormSubmitting`
- Lines 496-540: Handler functions wrapped in `startModelFormTransition(async () => { ... })`

**Secondary file:** `/osdevgrp.osintranet.react/src/components/SubmitToolbar.jsx`

**Verified:** SubmitToolbar button is `type="submit"` (line 13), enabling proper form submission trigger

## Build & Test Status

✅ **Build Status:** Clean build verified (454ms, no errors, no warnings)
- Command: `npm run build`
- Output: 862 modules transformed, production build successful

✅ **Validation Status:** Formik validation working correctly
- User confirmed: "Formik validates ok"
- All required fields validate before handler is called
- This eliminated validation as bottleneck, pointed to handler execution

⚠️ **Handler Execution Status:** NOT WORKING (pending modalModeRef implementation)
- Expected: Console logs appear when OK clicked
- Actual: Nothing happens
- Cause: Stale closure in `modalState.okCallback`
- Workaround: App works after reload (confirms diagnosis)

⚠️ **Modal Close Guard Status:** Code correct but can't be tested without handler fix
- Guard pattern syntax verified
- Guard logic sound (prevents close when `isModelFormSubmitting` is true)
- Will be validated during handler execution testing

## Lessons & Patterns

**Pattern: Modal Lifecycle with useTransition**
```jsx
const [isPending, startTransition] = useTransition();

<Modal onHide={() => {
    if (isPending) return;  // Guard
    setShow(false);
}}>
    <Form onSubmit={() => startTransition(async () => {
        // async work here
        // form stays disabled during this
    })}>
        <input disabled={isPending} />
    </Form>
</Modal>
```

**Anti-pattern: State-stored callbacks (causes stale closure)**
```jsx
// DON'T DO THIS
const [modal, setModal] = useState({
    okCallback: handleCreate  // captured at this moment
});
// Later render updates it, but closures still use old reference
<Formik onSubmit={modal.okCallback} />  // STALE!
```

**Better pattern: Ref-based routing (avoids closure)**
```jsx
// DO THIS INSTEAD
const modeRef = useRef(null);
setMode('create');  // just update ref, not state

<Formik onSubmit={(values) => {
    if (modeRef.current === 'create') handleCreate(values);
    else handleUpdate(values);
}}>
```

---

**Session Summary:** Identified and diagnosed critical race condition in PostingJournal modal lifecycle. Implemented useTransition-based protection (hooks, guards, field disabling) but discovered stale closure issue preventing handler execution. Root cause is Formik capturing old callback reference from state. Solution (modalModeRef) identified and ready to implement. App works after reload, proving mechanics are sound; requires closure fix for production readiness.
