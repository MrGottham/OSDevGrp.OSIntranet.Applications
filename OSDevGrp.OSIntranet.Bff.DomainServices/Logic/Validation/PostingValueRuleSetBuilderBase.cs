using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal abstract class PostingValueRuleSetBuilderBase : ValidationRuleSetBuilderBase
{
    #region Constructor

    protected PostingValueRuleSetBuilderBase(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, StaticTextKey staticTextKey, double minValue, double maxValue)
        : base(extendedValidationRuleSetBuilder)
    {
        StaticTextKey = staticTextKey;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    #endregion

    #region Properties

    protected StaticTextKey StaticTextKey { get; }

    protected double MinValue { get; }

    protected double MaxValue { get; }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder
            .WithMinValueRule(StaticTextKey, MinValue)
            .WithMaxValueRule(StaticTextKey, MaxValue)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}