using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PostingTextRuleSetBuilder : ValidationRuleSetBuilderBase, IPostingTextRuleSetBuilder
{
    #region Constructor

    public PostingTextRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.PostingText)
            .WithMinLengthRule(StaticTextKey.PostingText, AccountingRuleSetSpecifications.PostingTextMinLength)
            .WithMaxLengthRule(StaticTextKey.PostingText, AccountingRuleSetSpecifications.PostingTextMaxLength)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}