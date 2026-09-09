# Appending posting journal for an accounting

## General

We need to append/implement functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.WebApi
* **OUT OF SCOPE:** osdevgrp.osintranet.react

We need to create tests for functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.WebApi.Tests

**OUT OF SCOPE:** Note: No automated tests are needed for osdevgrp.osintranet.react as the React component will be validated through manual testing.

## Expose logic to add a posting line to a given accounting's posting journal from the BFF WebApi

### Business Goal

The WebApi should expose logic to add a posting line to a given accounting's posting journal. This should support the React application to add a posting line to a given accounting's posting journal then reload the posting journal.

### Implementation Bullets

#### OSDevGrp.OSIntranet.Bff.DomainServices

- [ ] Create `Features/Commands/Accounting/AccountingIdentificationRequestBase` abstract base class
  - [ ] Inherit from `RequestBase`
  - [ ] Add constructor: `protected AccountingIdentificationRequestBase(Guid requestId, int accountingNumber, ISecurityContext securityContext)`
  - [ ] Add read-only property: `public int AccountingNumber { get; }` (getter only)
- [ ] Create `Features/Commands/Accounting/AccountingIdentificationFeatureBase<TAccountingIdentificationRequest>` generic base class for accounting commands
  - [ ] Generic constraint: `where TAccountingIdentificationRequest : AccountingIdentificationRequestBase`
  - [ ] Implement `ICommandFeature<TAccountingIdentificationRequest>` 
  - [ ] Implement `IPermissionVerifiable<TAccountingIdentificationRequest>`
  - [ ] Add dependencies: `IPermissionChecker`, `IAccountingGateway`
  - [ ] Add protected property: `protected IPermissionChecker PermissionChecker { get; }` (getter only)
  - [ ] Add protected property: `protected IAccountingGateway AccountingGateway { get; }` (getter only)
  - [ ] Add method: `VerifyPermissionAsync()` - check authenticated + accounting access + accounting modifier role
  - [ ] Add abstract method: `ExecuteAsync(TAccountingIdentificationRequest request, CancellationToken cancellationToken)` - to be implemented by derived classes

#### OSDevGrp.OSIntranet.Bff.DomainServices.Tests

- [ ] Create `Features/Commands/Accounting/AccountingIdentificationFeatureBase/VerifyPermissionAsyncTests` test class
  - [ ] Test: `VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext()` - verify User property is accessed from ISecurityContext
  - [ ] Test: `VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext()` - test all authenticated states (8 test cases)
  - [ ] Test: `VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext()` - test all access states when authenticated
  - [ ] Test: `VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingModifierWasCalledOnPermissionChecker()` - verify IsAccountingModifier is called with user and accounting number
  - [ ] Test: `VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertIsAccountingModifierWasNotCalledOnPermissionChecker()` - verify IsAccountingModifier is NOT called when access denied
  - [ ] Test: `VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker()` - verify HasAccountingAccess is NOT called when not authenticated
  - [ ] Test: `VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertIsAccountingModifierWasNotCalledOnPermissionChecker()` - verify IsAccountingModifier is NOT called when not authenticated
  - [ ] Test: `VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue()` - test return value with all permission combinations (8 test cases: only true when all three checks pass)
- [ ] Create `Features/Commands/Accounting/AccountingIdentificationFeatureBase/ExecuteAsyncTests` test class
  - [ ] Test: `ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledWithGivenRequest()` - verify ExecuteAsync abstract method receives the request
  - [ ] Test: `ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledWithGivenCancellationToken()` - verify ExecuteAsync abstract method receives the cancellation token
