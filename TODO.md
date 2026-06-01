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
* ⏳ Register IPostingJournalLineIdentifierRuleSetBuilder with PostingJournalLineIdentifierRuleSetBuilder in AddDomainServices at OSDevGrp.OSIntranet.Bff.DomainServices.ServiceCollectionExtensions
* ⏳ Do not use the IPostingJournalLineIdentifierRuleSetBuilder in any logic yet

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
