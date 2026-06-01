using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PostingJournalLineIdentifierRuleSetBuilder : ValidationRuleSetBuilderBase, IPostingJournalLineIdentifierRuleSetBuilder
{
    #region Constructor

    public PostingJournalLineIdentifierRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.PostingJournalIdentifier)
            .WithMinLengthRule(StaticTextKey.PostingJournalIdentifier, AccountingRuleSetSpecifications.PostingLineIdentificationMinLength)
            .WithMaxLengthRule(StaticTextKey.PostingJournalIdentifier, AccountingRuleSetSpecifications.PostingLineIdentificationMaxLength)
            .WithPatternRule(StaticTextKey.PostingJournalIdentifier, AccountingRuleSetSpecifications.PostingLineIdentificationRegexPattern)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}