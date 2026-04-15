using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class LetterHeadNumberRuleSetBuilder : ValidationRuleSetBuilderBase, ILetterHeadNumberRuleSetBuilder
{
    #region Constructor

    public LetterHeadNumberRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.LetterHeadNumber)
            .WithShouldBeIntegerRule(StaticTextKey.LetterHeadNumber)
            .WithRangeRule(StaticTextKey.LetterHeadNumber, LetterHeadRuleSetSpecifications.LetterHeadNumberMinValue, LetterHeadRuleSetSpecifications.LetterHeadNumberMaxValue)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}