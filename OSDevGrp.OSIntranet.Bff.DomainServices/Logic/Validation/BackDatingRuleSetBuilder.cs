using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class BackDatingRuleSetBuilder : ValidationRuleSetBuilderBase, IBackDatingRuleSetBuilder
{
    #region Constructor

    public BackDatingRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.BackDating)
            .WithRangeRule(StaticTextKey.BackDating, AccountingRuleSetSpecifications.BackDatingMinValue, AccountingRuleSetSpecifications.BackDatingMaxValue)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}