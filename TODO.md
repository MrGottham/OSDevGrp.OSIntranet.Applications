# Appending posting journal for an accounting

## General

We need to append functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* OSDevGrp.OSIntranet.Bff.ServiceGateways
* OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices
* OSDevGrp.OSIntranet.Bff.WebApi

We need to create tests and test data for functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* OSDevGrp.OSIntranet.Bff.WebApi.Tests

⚠️ **Service Layer Integration**: Do not register or call the three new feature classes (AccountSummary, BudgetAccountSummary, ContactAccountSummary) in the service layer yet. They must wait until all base classes and features are completed and tested.

## Shared summary functionality

✅ **COMPLETED - Phase 1**: The following base classes and test infrastructure have been implemented (2026-08-05).

* ✅ **AccountIdentificationRequestBase** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting
  * ✅ Inherits directly from AccountingIdentificationRequestBase
  * ✅ Has AccountNumber (string) property
  * ✅ AccountNumber property set by constructor
  * **File**: AccountIdentificationRequestBase.cs

* ✅ **AccountIdentificationResponseBase** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting
  * ✅ Inherits directly from AccountingIdentificationResponseBase
  * ✅ Generic parameters: <TModel, TDynamicTexts>
  * **File**: AccountIdentificationResponseBase.cs

* ✅ **AccountIdentificationFeatureBase** in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting
  * ✅ Inherits directly from AccountingIdentificationFeatureBase
  * ✅ 6 generic type parameters with correct constraints:
    - TAccountIdentificationRequest : AccountIdentificationRequestBase
    - TAccountIdentificationResponse : AccountIdentificationResponseBase<TModel, TDynamicTexts>
    - TModel : class
    - TDynamicTexts : IDynamicTexts
    - TDynamicTextsBuilder : IDynamicTextsBuilder<TModel, TDynamicTexts>
    - TValidationRuleSetBuilder : IValidationRuleSetBuilder
  * ✅ No method overrides needed - inherits ExecuteAsync orchestration from parent
  * **File**: AccountIdentificationFeatureBase.cs

* ✅ **Test Infrastructure for AccountIdentificationFeatureBase**
  * ✅ AccountIdentificationFeatureTestBase with static factory methods
  * ✅ MyAccountIdentificationRequest concrete test class
  * ✅ MyAccountIdentificationResponse concrete test class
  * ✅ VerifyPermissionAsyncTests (9 tests covering all permission scenarios)
  * ✅ ExecuteAsyncTests (16 tests covering execution paths)
  * **Total**: 39 unit tests, all passing
  * **Files**: AccountIdentificationFeatureTestBase.cs, MyAccountIdentificationRequest.cs, MyAccountIdentificationResponse.cs, VerifyPermissionAsyncTests.cs, ExecuteAsyncTests.cs

**Phase 1 Verification**:
- ✅ Solution builds: 0 errors, 0 warnings
- ✅ All 1,902 unit tests pass (39 new tests + 1,863 existing)
- ✅ No regressions
- ✅ Implementation diary complete: docs/diary/2026-07-29-account-identification-features.md
- ✅ Git commit: "Phase 1: Implement account identification base classes with comprehensive unit tests"

**Ready for Phase 2** ✅

---

## Adding account summary feature

**Phase 2**: Ready to implement - all prerequisites from Phase 1 are complete. The following three feature classes can now be implemented:

* Make the public class AccountSummaryRequest in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary which should
  * inherit directly AccountIdentificationRequestBase
* Make the public class AccountSummaryResponse in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary which should
  * inherit directly AccountIdentificationResponseBase with TModel as AccountModel and TDynamicTexts as IAccountTexts
* Make the internal class AccountSummaryFeature in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary which should
  * inherit directly from AccountIdentificationFeatureBase using AccountSummaryRequest, AccountSummaryResponse, AccountModel, IAccountTexts, IAccountTextsBuilder and IEmptyRuleSetBuilder as generic arguments
  * should use GetAccountAsync (or verify correct method name) at IAccountingGateway to resolve the model in GetModelAsync
  * build a AccountSummaryResponse in BuildResponseAsync
  * should return an empty dictionary in GetStaticTextSpecifications
* Make test for AccountSummaryFeature simmular to test in AccountingSummaryFeature

## Adding budget account summary feature

* Make the public class BudgetAccountSummaryRequest in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary which should
  * inherit directly AccountIdentificationRequestBase
* Make the public class BudgetAccountSummaryResponse in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary which should
  * inherit directly AccountIdentificationResponseBase with TModel as BudgetAccountModel and TDynamicTexts as IBudgetAccountTexts
* Make the internal class BudgetAccountSummaryFeature in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary which should
  * inherit directly from AccountIdentificationFeatureBase using BudgetAccountSummaryRequest, BudgetAccountSummaryResponse, BudgetAccountModel, IBudgetAccountTexts, IBudgetAccountTextsBuilder and IEmptyRuleSetBuilder as generic arguments
  * should use GetBudgetAccountAsync (or verify correct method name) at IAccountingGateway to resolve the model in GetModelAsync
  * build a BudgetAccountSummaryResponse in BuildResponseAsync
  * should return an empty dictionary in GetStaticTextSpecifications
* Make test for BudgetAccountSummaryFeature simmular to test in AccountingSummaryFeature

## Adding contact account summary feature

* Make the public class ContactAccountSummaryRequest in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary which should
  * inherit directly AccountIdentificationRequestBase
* Make the public class ContactAccountSummaryResponse in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary which should
  * inherit directly AccountIdentificationResponseBase with TModel as ContactAccountModel and TDynamicTexts as IContactAccountTexts
* Make the internal class ContactAccountSummaryFeature in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary which should
  * inherit directly from AccountIdentificationFeatureBase using ContactAccountSummaryRequest, ContactAccountSummaryResponse, ContactAccountModel, IContactAccountTexts, IContactAccountTextsBuilder and IEmptyRuleSetBuilder as generic arguments
  * should use GetContactAccountAsync (or verify correct method name) at IAccountingGateway to resolve the model in GetModelAsync
  * build a ContactAccountSummaryResponse in BuildResponseAsync
  * should return an empty dictionary in GetStaticTextSpecifications
* Make test for ContactAccountSummaryFeature simmular to test in AccountingSummaryFeature
