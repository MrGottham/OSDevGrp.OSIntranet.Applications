using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PostingReferenceRuleSetBuilder : ValidationRuleSetBuilderBase, IPostingReferenceRuleSetBuilder
{
    #region Constructor

    public PostingReferenceRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithMinLengthRule(StaticTextKey.PostingReference, AccountingRuleSetSpecifications.PostingReferenceMinLength)
            .WithMaxLengthRule(StaticTextKey.PostingReference, AccountingRuleSetSpecifications.PostingReferenceMaxLength)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}