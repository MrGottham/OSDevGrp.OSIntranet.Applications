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

⚠️ **Prerequisites**: The following base classes must be completed first before AccountSummaryFeature can be implemented.

* Make the public abstract class named AccountIdentificationRequestBase in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting which should
  * inherit directly from AccountingIdentificationRequestBase
  * have a AccountNumber (string) getter
  * the AccountNumber property should be set by the constructor
* Make the public abstract class named AccountIdentificationResponseBase in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting which should
  * inherit directly from AccountingIdentificationResponseBase
* Make the internal abstract class named AccountIdentificationFeatureBase in OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting which should
  * inherit directly from AccountingIdentificationFeatureBase
  * should have the generic argument TAccountIdentificationRequest as TAccountingIdentificationRequest where TAccountIdentificationRequest is AccountIdentificationRequestBase
  * should have the generic argument TAccountIdentificationResponse as TAccountingIdentificationResponse where TAccountIdentificationResponse is AccountIdentificationResponseBase
  * should have TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder as is in AccountingIdentificationResponseBase
* Make test for AccountIdentificationFeatureBase simmular to test in AccountingIdentificationFeatureTestBase

## Adding account summary feature

⚠️ **Do not implement yet**: These three feature classes must wait until AccountIdentificationFeatureBase and its prerequisites are completed.

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
