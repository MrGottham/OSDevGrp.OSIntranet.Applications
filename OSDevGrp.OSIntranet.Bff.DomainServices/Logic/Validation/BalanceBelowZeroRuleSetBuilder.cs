using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class BalanceBelowZeroRuleSetBuilder : ValidationRuleSetBuilderBase, IBalanceBelowZeroRuleSetBuilder
{
    #region Private variables

    private readonly IStaticTextProvider _staticTextProvider;

    #endregion

    #region Constructor

    public BalanceBelowZeroRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, IStaticTextProvider staticTextProvider)
        : base(extendedValidationRuleSetBuilder)
    {
        _staticTextProvider = staticTextProvider;
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IValueSpecification<int>[] validValues =
        [
            ValueSpecification<int>.Create(AccountingRuleSetSpecifications.BalanceBelowZeroDebtorsValue, await _staticTextProvider.GetStaticTextAsync(StaticTextKey.Debtors, StaticTextKey.Debtors.DefaultArguments(), formatProvider, cancellationToken)),
            ValueSpecification<int>.Create(AccountingRuleSetSpecifications.BalanceBelowZeroCreditorsValue, await _staticTextProvider.GetStaticTextAsync(StaticTextKey.Creditors, StaticTextKey.Creditors.DefaultArguments(), formatProvider, cancellationToken)),
        ];

        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.BalanceBelowZero)
            .WithOneOfRule(StaticTextKey.BalanceBelowZero, validValues)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}