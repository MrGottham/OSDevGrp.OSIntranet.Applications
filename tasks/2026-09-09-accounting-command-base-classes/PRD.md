# PRD: Accounting Command Base Classes

## Problem

The BFF DomainServices layer has a well-established pattern for **query features** (through `AccountingIdentificationFeatureBase<>` in the Queries directory), which provides a consistent, reusable foundation for read operations on accounting data. However, there is **no parallel pattern for command features** that modify accounting data. Without base classes, developers building new accounting commands (like `AppendPostingLine`) must:

1. Implement permission checking from scratch, risking inconsistency in the permission logic (e.g., some commands might forget to check `IsAccountingModifier`)
2. Duplicate boilerplate code for dependency injection and interface implementation
3. Have no clear contract or naming convention for accounting-related commands

This inconsistency slows development, increases the surface area for bugs (especially around authorization), and makes the codebase harder to navigate and maintain.

**Why now:** The upcoming `AppendPostingLine` feature needs a structured foundation; building these base classes first will unblock that work and establish the pattern for all future accounting commands.

## Relevant Codebase

### Existing Query Pattern (Template for Commands)

**Query Base Class:** [Features/Queries/Accounting/AccountingIdentificationFeatureBase.cs](../../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationFeatureBase.cs)
- Generic base: `AccountingIdentificationFeatureBase<TRequest, TResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder>`
- Inherits from `PageFeatureBase<TRequest, TResponse, TModel>` (handles response building, dynamic text, validation rules)
- Implements `IPermissionVerifiable<TRequest>` — method: `VerifyPermissionAsync(ISecurityContext, TRequest, CancellationToken)`
- Permission logic (line 70–72): `IsAuthenticated(user) && HasAccountingAccess(user) && IsAccountingViewer(user, accountingNumber)`
- Dependencies injected: `IPermissionChecker`, `IAccountingGateway`, `IStaticTextProvider`, builder types

**Query Request Base:** [Features/Queries/Accounting/AccountingIdentificationRequestBase.cs](../../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingIdentificationRequestBase.cs)
- Inherits from `PageRequestBase`
- Properties: `AccountingNumber`, `StatusDate`, `FormatProvider`

### Existing Command Example (Simpler Pattern, Not a Base)

**Command Feature:** [Features/Commands/Security/GenerateVerification/GenerateVerificationFeature.cs](../../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Security/GenerateVerification/GenerateVerificationFeature.cs)
- Concrete class (not abstract base)
- Implements `ICommandFeature<GenerateVerificationRequest>` — method: `ExecuteAsync(TRequest, CancellationToken)`
- Implements `IPermissionVerifiable<GenerateVerificationRequest>` — method: `VerifyPermissionAsync(...)`
- Dependencies injected as private fields; no exposure to derived classes
- Permission logic is simple: `IsAuthenticated(user)` only

### CQS Interfaces

**[ICommandFeature\<TRequest\>](../../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Cqs/ICommandFeature.cs):**
```csharp
public interface ICommandFeature<TRequest> where TRequest : IRequest
{
    Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
```

**[IPermissionVerifiable\<TRequest\>](../../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Cqs/IPermissionVerifiable.cs):**
```csharp
public interface IPermissionVerifiable<TRequest> where TRequest : IRequest
{
    Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, TRequest request, CancellationToken cancellationToken = default);
}
```

### Security Model

**[IPermissionChecker](../../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Security/IPermissionCheker.cs):** Core interface with methods:
- `IsAuthenticated(ClaimsPrincipal user)` → bool
- `HasAccountingAccess(ClaimsPrincipal user)` → bool
- `IsAccountingModifier(ClaimsPrincipal user, int? accountingNumber)` → bool — **Used for commands** (vs. `IsAccountingViewer` for queries)
- `IsAccountingAdministrator`, `IsAccountingCreator`, `HasCommonDataAccess` — other roles

### Directory Structure

**Source:** `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/`
- Currently exists but empty (except `Accounting/` folder)
- Will contain: `AccountingIdentificationRequestBase.cs`, `AccountingIdentificationFeatureBase.cs`

**Tests:** `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Commands/Accounting/AccountingIdentificationFeatureBase/`
- Will contain: `VerifyPermissionAsyncTests.cs`, `ExecuteAsyncTests.cs`

## Goal

Provide **reusable, minimal base classes** that establish a consistent pattern for accounting command features in the BFF DomainServices layer. When complete:

- Developers can build new accounting commands by inheriting from `AccountingIdentificationFeatureBase<TRequest>`, ensuring permission logic is correct and consistent
- Permission checks are centralized and testable: authentication, accounting access, **modifier role** (as distinct from viewer role in queries)
- Dependencies (`IPermissionChecker`, `IAccountingGateway`) are injected into the base class and exposed as protected properties, available to all derived commands
- The request base class provides a minimal contract for all accounting command requests: `AccountingNumber` and security context
- The base is simple and extensible: `VerifyPermissionAsync()` is virtual, allowing derived classes to override if specialized logic is needed
- Tests verify that permission checks work correctly across all combinations and that abstract methods receive parameters correctly

## User Stories

1. **As a developer** building a new accounting command (e.g., `AppendPostingLine`), I want to inherit from a command base class so that I don't have to duplicate permission-checking logic or dependency injection code, reducing boilerplate and ensuring consistency.

2. **As a code reviewer**, I want to see a clear, reusable pattern for accounting commands so that I can quickly recognize when permission checks are missing or implemented incorrectly.

3. **As a maintainer**, I want a single place to update the accounting permission logic (the base class) so that changes apply consistently to all accounting commands, reducing the risk of bugs spreading across the codebase.

## Acceptance Criteria

### 1. Request Base Class
- [ ] File `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationRequestBase.cs` exists and is an abstract class
- [ ] Inherits from `RequestBase`
- [ ] Has constructor: `protected AccountingIdentificationRequestBase(Guid requestId, int accountingNumber, ISecurityContext securityContext)` that calls base constructor and sets `AccountingNumber`
- [ ] Has read-only public property: `public int AccountingNumber { get; }`
- [ ] Uses `#region` blocks: Constructor, Properties, Methods (matching project convention)
- [ ] Compiles without errors; `dotnet build` passes

### 2. Feature Base Class
- [ ] File `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationFeatureBase.cs` exists and is an abstract generic class
- [ ] Generic constraint: `where TAccountingIdentificationRequest : AccountingIdentificationRequestBase`
- [ ] Implements `ICommandFeature<TAccountingIdentificationRequest>`
- [ ] Implements `IPermissionVerifiable<TAccountingIdentificationRequest>`
- [ ] Constructor accepts and stores `IPermissionChecker` and `IAccountingGateway` as dependencies
- [ ] Has protected properties (getter-only): `protected IPermissionChecker PermissionChecker { get; }` and `protected IAccountingGateway AccountingGateway { get; }`
- [ ] Implements `VerifyPermissionAsync()` (public, virtual) that:
  - Accesses `securityContext.User`
  - Calls `PermissionChecker.IsAuthenticated(user)` first
  - Calls `PermissionChecker.HasAccountingAccess(user)` only if authenticated
  - Calls `PermissionChecker.IsAccountingModifier(user, request.AccountingNumber)` only if authenticated and has access
  - Returns `true` only if all three checks pass
  - Runs permission logic on a background thread (via `Task.Run()`, consistent with query pattern)
- [ ] Declares abstract method: `public abstract Task ExecuteAsync(TAccountingIdentificationRequest request, CancellationToken cancellationToken)` for derived classes to implement
- [ ] Uses `#region` blocks: Constructor, Properties, Methods
- [ ] Compiles without errors; `dotnet build` passes

### 3. Permission Verification Tests
- [ ] Test class `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Commands/Accounting/AccountingIdentificationFeatureBase/VerifyPermissionAsyncTests.cs` exists
- [ ] Has `[TestFixture]` attribute and `[SetUp]` method (NUnit 4.6.1 pattern)
- [ ] Tests verify User property is accessed from `ISecurityContext`:
  - [ ] Test `VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext()` passes
- [ ] Tests verify `IsAuthenticated` is called correctly:
  - [ ] Test `VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext()` passes with mock assertions
- [ ] Tests verify `HasAccountingAccess` is called only when authenticated:
  - [ ] Test `VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext()` passes
  - [ ] Test `VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker()` passes
- [ ] Tests verify `IsAccountingModifier` is called only when authenticated AND has access:
  - [ ] Test `VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingModifierWasCalledOnPermissionChecker()` passes
  - [ ] Test `VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertIsAccountingModifierWasNotCalledOnPermissionChecker()` passes
  - [ ] Test `VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertIsAccountingModifierWasNotCalledOnPermissionChecker()` passes
- [ ] Tests verify return value logic:
  - [ ] Test `VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue()` passes with all 8 permission combinations (F/F/F, F/F/T, F/T/F, F/T/T, T/F/F, T/F/T, T/T/F, T/T/T) — returns true only for T/T/T
- [ ] All tests use Moq for mocks, AutoFixture for test data, and follow NUnit patterns from the codebase
- [ ] Tests are tagged `[Category("UnitTest")]`
- [ ] All tests pass: `dotnet test OSDevGrp.OSIntranet.Bff.DomainServices.Tests --filter "FullyQualifiedName~VerifyPermissionAsyncTests"`

### 4. ExecuteAsync Parameter Tests
- [ ] Test class `OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Features/Commands/Accounting/AccountingIdentificationFeatureBase/ExecuteAsyncTests.cs` exists
- [ ] Has `[TestFixture]` attribute and `[SetUp]` method
- [ ] Uses a concrete test implementation of the feature base that captures `ExecuteAsync` parameters
- [ ] Test `ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledWithGivenRequest()` verifies the request parameter is passed correctly
- [ ] Test `ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledWithGivenCancellationToken()` verifies the cancellation token is passed correctly
- [ ] Tests are tagged `[Category("UnitTest")]`
- [ ] All tests pass: `dotnet test OSDevGrp.OSIntranet.Bff.DomainServices.Tests --filter "FullyQualifiedName~ExecuteAsyncTests and FullyQualifiedName~AccountingIdentificationFeatureBase"`

### 5. Integration & Quality
- [ ] Full solution builds: `dotnet build OSDevGrp.OSIntranet.Applications.sln` exits with code 0
- [ ] All unit tests pass: `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"` exits with code 0
- [ ] Code style follows project conventions:
  - Namespaces: `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting`
  - `#region` blocks: Constructor, Properties, Methods
  - Interfaces implemented in order: `ICommandFeature<>`, then `IPermissionVerifiable<>`
  - Private fields: `_camelCase`
  - Methods: `PascalCase`
  - Comments/XML docs follow existing patterns (minimal; code is self-documenting)

## Scope

### In Scope
- Create `AccountingIdentificationRequestBase` abstract base class with minimal properties (accounting number, security context only)
- Create `AccountingIdentificationFeatureBase<TAccountingIdentificationRequest>` abstract generic base class with dependency injection, permission checks, and contract for `ExecuteAsync`
- Implement `VerifyPermissionAsync()` using the permission flow: `IsAuthenticated && HasAccountingAccess && IsAccountingModifier` (consistent with the query pattern but using Modifier instead of Viewer)
- Make `VerifyPermissionAsync()` virtual to allow derived classes to override if needed
- Write comprehensive unit tests for permission verification (all 8 permission combinations) and parameter passing
- Ensure code builds and all tests pass

### Out of Scope
- **No implementation of `AppendPostingLine` command feature** — that will inherit from these base classes in a future task
- **No WebApi endpoint** for `AppendPostingLine` — that's part of the WebApi layer and deferred
- **No changes to ServiceGateways or their interfaces** — those are already established
- **No changes to DomainServices.Interfaces** — the existing CQS interfaces (`ICommandFeature`, `IPermissionVerifiable`) are sufficient
- **No React UI implementation** — manual testing only for the final feature
- **No performance optimization or caching** — base classes remain simple and minimal

## Risks

1. **Permission Logic Inconsistency:** If `VerifyPermissionAsync()` is not properly tested with all permission combinations, derived commands may inherit a buggy permission check. *Mitigation:* Comprehensive test matrix (8 cases) with explicit assertions on mock call sequences.

2. **Short-Circuit Logic:** If the implementation doesn't short-circuit permission checks (e.g., still calls `IsAccountingModifier` even when `IsAuthenticated` is false), it could mask bugs in derived commands or test understanding. *Mitigation:* Tests explicitly verify that later checks are NOT called when earlier checks fail (e.g., `AssertIsAccountingModifierWasNotCalledOnPermissionChecker`).

3. **Virtual Method Override Risk:** Making `VerifyPermissionAsync()` virtual allows derived classes to override but also allows them to accidentally break the permission contract. *Mitigation:* Code review checklist; the base implementation is sound and rarely needs overriding; future `AppendPostingLine` and similar features should inherit without override unless there's a clear, documented reason.

4. **Integration with DI Container:** When derived command classes are registered in the DI container, they must inject `IPermissionChecker` and `IAccountingGateway` into the base class constructor. If the container is misconfigured, derived commands will fail at runtime. *Mitigation:* Clear examples in derived class implementation (when `AppendPostingLine` is built) and existing `GenerateVerificationFeature` pattern shows this works.

5. **Inheritance Chain Complexity:** Generic constraints and multiple inheritance levels (`TRequest : AccountingIdentificationRequestBase`) can make error messages confusing if type constraints are violated. *Mitigation:* Clear error messages from compiler; tests will catch misuse quickly.

6. **Future Expansion:** If a future accounting command needs different permission logic (e.g., only authentication, no accounting access check), they might be tempted to bypass the base class entirely. *Mitigation:* The virtual `VerifyPermissionAsync()` allows customization; clear documentation in code comments; code review to flag unusual permission patterns.
