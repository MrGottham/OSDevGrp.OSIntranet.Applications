using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class AccountingNumberRuleSetBuilder : ValidationRuleSetBuilderBase, IAccountingNumberRuleSetBuilder
{
    #region Constructor

    public AccountingNumberRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.AccountingNumber)
            .WithShouldBeIntegerRule(StaticTextKey.AccountingNumber)
            .WithRangeRule(StaticTextKey.AccountingNumber, AccountingRuleSetSpecifications.AccountingNumberMinValue, AccountingRuleSetSpecifications.AccountingNumberMaxValue)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}