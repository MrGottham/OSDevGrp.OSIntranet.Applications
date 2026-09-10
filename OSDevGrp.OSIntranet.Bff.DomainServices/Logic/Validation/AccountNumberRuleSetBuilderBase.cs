using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal abstract class AccountNumberRuleSetBuilderBase : ValidationRuleSetBuilderBase
{
    #region Constructor

    protected AccountNumberRuleSetBuilderBase(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, StaticTextKey staticTextKey, bool required)
        : base(extendedValidationRuleSetBuilder)
    {
        StaticTextKey = staticTextKey;
        Required = required;
    }

    #endregion

    #region Properties

    protected StaticTextKey StaticTextKey { get; }

    protected bool Required { get; }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        var builder = ExtendedValidationRuleSetBuilder;

        if (Required)
        {
            builder = builder.WithRequiredValueRule(StaticTextKey);
        }

        return await builder
            .WithMinLengthRule(StaticTextKey, AccountingRuleSetSpecifications.AccountNumberMinLength)
            .WithMaxLengthRule(StaticTextKey, AccountingRuleSetSpecifications.AccountNumberMaxLength)
            .WithPatternRule(StaticTextKey, AccountingRuleSetSpecifications.AccountNumberRegexPattern)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}