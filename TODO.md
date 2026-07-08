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

## Add validation rules for the posting line identification within a posting journal line

**✓ IMPLEMENTED** - Commit: 3c6219af

* ✓ Move the constants PostingLineIdentificationMinLength, PostingLineIdentificationMaxLength and PostingLineIdentificationRegexPattern from ValidationValues at OSDevGrp.OSIntranet.Bff.WebApi.Shared to AccountingRuleSetSpecifications at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation and make them public
* ✓ Make sure that OSDevGrp.OSIntranet.Bff.WebApi now uses the moved constants in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation
* ✓ Add the StaticTextKey named PostingJournalIdentifier to the static text enum
* ✓ Add the static text "Identifikation" in the StaticTextProvider for the static text key PostingJournalIdentifier
* ✓ Add tests to ensure that the static text key PostingJournalIdentifier returns the correct text
* ✓ Add the interface IPostingJournalLineIdentifierRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement PostingJournalLineIdentifierRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit ValidationRuleSetBuilderBase and add following rules:
  * ✓ a required value rule using WithRequiredValueRule for the Identifier on the posting journal
  * ✓ a min length rule using WithMinLengthRule for the Identifier on the posting journal where min length is defined by the constant PostingLineIdentificationMinLength
  * ✓ a max length rule using WithMaxLengthRule for the Identifier on the posting journal where max length is defined by the constant PostingLineIdentificationMaxLength
  * ✓ a pattern rule using WithPatternRule for the Identifier on the posting journal where pattern is defined by the constant PostingLineIdentificationRegexPattern
* ✓ Implement tests for the PostingJournalLineIdentifierRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement PostingJournalLineIdentifierRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IPostingJournalLineIdentifierRuleSetBuilder with PostingJournalLineIdentifierRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Do not use the IPostingJournalLineIdentifierRuleSetBuilder in any logic yet

## Add validation rules for the posting date within a posting journal line

**✓ IMPLEMENTED** - Commit: 3c6219af

* ✓ Add the interface IPostingDateRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement PostingDateRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit ValidationRuleSetBuilderBase and add following rules:
  * ✓ a required value rule using WithRequiredValueRule for the PostingDate on the posting journal
  * ✓ a DateTimeOffset rule using WithMinValueRule where the min value should be the date given by GetUtcNow on the class dependency TimeProvider minus the number days given by the constant BackDatingMaxValue at AccountingRuleSetSpecifications
  * ✓ a DateTimeOffset rule using WithMaxValueRule where the max value should be the date given by GetUtcNow on the class dependency TimeProvider
* ✓ Implement tests for the PostingDateRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement PostingDateRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IPostingDateRuleSetBuilder with PostingDateRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Do not use the IPostingDateRuleSetBuilder in any logic yet

## Add validation rules for posting reference date within a posting journal line

**✓ IMPLEMENTED** - Commit: 133f2a70

* ✓ Move the constants PostingReferenceMinLength and PostingReferenceMaxLength from ValidationValues at OSDevGrp.OSIntranet.Bff.WebApi.Shared to AccountingRuleSetSpecifications at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation and make them public
* ✓ Make sure that OSDevGrp.OSIntranet.Bff.WebApi now uses the moved constants in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation
* ✓ Add the interface IPostingReferenceRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement PostingReferenceRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit ValidationRuleSetBuilderBase and add following rules:
  * ✓ a min length rule using WithMinLengthRule for the PostingReference on the posting journal where min length is defined by the constant PostingReferenceMinLength
  * ✓ a max length rule using WithMaxLengthRule for the PostingReference on the posting journal where max length is defined by the constant PostingReferenceMaxLength
* ✓ Implement tests for the PostingReferenceRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement PostingReferenceRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IPostingReferenceRuleSetBuilder with PostingReferenceRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Do not use the IPostingReferenceRuleSetBuilder in any logic yet

## Add validation rules for account number, budget account number and contact account number within a posting journal line

**✓ IMPLEMENTED** - Commit: d3d99ec2

* ✓ Move the constants AccountNumberMinLength, AccountNumberMaxLength and AccountNumberRegexPattern from ValidationValues at OSDevGrp.OSIntranet.Bff.WebApi.Shared to AccountingRuleSetSpecifications at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation and make them public
* ✓ Make sure that OSDevGrp.OSIntranet.Bff.WebApi now uses the moved constants in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation
* ✓ Implement the abstact AccountNumberRuleSetBuilderBase as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit ValidationRuleSetBuilderBase and add following rules:
  * ✓ a required value rule using WithRequiredValueRule for the static text key given by the constructor when the required argument given in the contructor is true; otherwise ship this validation rule
  * ✓ a min length rule using WithMinLengthRule for the static text key given by the constructor where min length is defined by the constant AccountNumberMinLength
  * ✓ a max length rule using WithMaxLengthRule for the static text key given by the constructor where max length is defined by the constant AccountNumberMaxLength
  * ✓ a pattern rule using WithPatternRule for the static text key given by the constructor where pattern is defined by the constant AccountNumberRegexPattern
* ✓ Implement tests for the AccountNumberRuleSetBuilderBase in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Add the interface IAccountNumberRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement AccountNumberRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit AccountNumberRuleSetBuilderBase with the static text key Account and set required argument to true
* ✓ Implement tests for the AccountNumberRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement AccountNumberRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IAccountNumberRuleSetBuilder with AccountNumberRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Add the interface IBudgetAccountNumberRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement BudgetAccountNumberRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit AccountNumberRuleSetBuilderBase with the static text key BudgetAccount and set required argument to false
* ✓ Implement tests for the BudgetAccountNumberRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement BudgetAccountNumberRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IBudgetAccountNumberRuleSetBuilder with BudgetAccountNumberRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Add the interface IContactAccountNumberRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement ContactAccountNumberRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit AccountNumberRuleSetBuilderBase with the static text key ContactAccount and set required argument to false
* ✓ Implement tests for the ContactAccountNumberRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement ContactAccountNumberRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IContactAccountNumberRuleSetBuilder with ContactAccountNumberRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Do not use the IAccountNumberRuleSetBuilder, IBudgetAccountNumberRuleSetBuilder nor IContactAccountNumberRuleSetBuilder in any logic yet

## Add validation rules for posting text date within a posting journal line

**✓ IMPLEMENTED** - Commit: 1f466506

* ✓ Move the constants PostingTextMinLength and PostingTextMaxLength from ValidationValues at OSDevGrp.OSIntranet.Bff.WebApi.Shared to AccountingRuleSetSpecifications at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation and make them public
* ✓ Make sure that OSDevGrp.OSIntranet.Bff.WebApi now uses the moved constants in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation
* ✓ Add the interface IPostingTextRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement PostingTextRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit ValidationRuleSetBuilderBase and add following rules:
  * ✓ a required value rule using WithRequiredValueRule for the PostingText on the posting journal
  * ✓ a min length rule using WithMinLengthRule for the PostingText on the posting journal where min length is defined by the constant PostingTextMinLength
  * ✓ a max length rule using WithMaxLengthRule for the PostingText on the posting journal where max length is defined by the constant PostingTextMaxLength
* ✓ Implement tests for the PostingTextRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement PostingTextRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IPostingTextRuleSetBuilder with PostingTextRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Do not use the IPostingTextRuleSetBuilder in any logic yet

## Add validation rules for debit and credit within a posting journal line

**✓ IMPLEMENTED** - Commit: 2e279c3f

* ✓ Move the constants DebitMinValue, DebitMaxValue, CreditMinValue and CreditMaxValue from ValidationValues at OSDevGrp.OSIntranet.Bff.WebApi.Shared to AccountingRuleSetSpecifications at OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation and make them public
* ✓ Make sure that OSDevGrp.OSIntranet.Bff.WebApi now uses the moved constants in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation
* ✓ Implement the abstract PostingValueRuleSetBuilderBase as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit ValidationRuleSetBuilderBase and add following rules:
  * ✓ a min value rule using WithMinValueRule for the static text key given by the constructor and min value (double) is given by the constructor
  * ✓ a max value rule using WithMaxValueRule for the static text key given by the constructor and max value (double) is given by the constructor
* ✓ Implement tests for the PostingValueRuleSetBuilderBase in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Add the interface IDebitRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement DebitRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit PostingValueRuleSetBuilderBase with the static text key Debit and use DebitMinValue as min value and DebitMaxValue as max value
* ✓ Implement tests for the DebitRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement DebitRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register IDebitRuleSetBuilder with DebitRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Add the interface ICreditRuleSetBuilder which implements the interface IValidationRuleSetBuilder to the validation logic in OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* ✓ Implement CreditRuleSetBuilder as validation logic in OSDevGrp.OSIntranet.Bff.DomainServices. This ruleset builder should inherit PostingValueRuleSetBuilderBase with the static text key Credit and use CreditMinValue as min value and CreditMaxValue as max value
* ✓ Implement tests for the CreditRuleSetBuilder in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* ✓ Implement CreditRuleSetBuilderMockExtensions in validation tests at OSDevGrp.OSIntranet.Bff.DomainServices.Tests so we can mockup the rule set builder
* ✓ Register ICreditRuleSetBuilder with CreditRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ✓ Do not use the IDebitRuleSetBuilder nor ICreditRuleSetBuilder in any logic yet
