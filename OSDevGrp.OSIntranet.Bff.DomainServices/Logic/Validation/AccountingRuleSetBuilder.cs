using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class AccountingRuleSetBuilder : ValidationRuleSetBuilderBase, IAccountingRuleSetBuilder
{
    #region Private variables

    private readonly IAccountingNumberRuleSetBuilder _accountingNumberRuleSetBuilder;
    private readonly IAccountingNameRuleSetBuilder _accountingNameRuleSetBuilder;
    private readonly ILetterHeadNumberRuleSetBuilder _letterHeadNumberRuleSetBuilder;
    private readonly IBalanceBelowZeroRuleSetBuilder _balanceBelowZeroRuleSetBuilder;
    private readonly IBackDatingRuleSetBuilder _backDatingRuleSetBuilder;

    #endregion

    #region Constructor

    public AccountingRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, IAccountingNumberRuleSetBuilder accountingNumberRuleSetBuilder, IAccountingNameRuleSetBuilder accountingNameRuleSetBuilder, ILetterHeadNumberRuleSetBuilder letterHeadNumberRuleSetBuilder, IBalanceBelowZeroRuleSetBuilder balanceBelowZeroRuleSetBuilder, IBackDatingRuleSetBuilder backDatingRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
        _accountingNumberRuleSetBuilder = accountingNumberRuleSetBuilder;
        _accountingNameRuleSetBuilder = accountingNameRuleSetBuilder;
        _letterHeadNumberRuleSetBuilder = letterHeadNumberRuleSetBuilder;
        _balanceBelowZeroRuleSetBuilder = balanceBelowZeroRuleSetBuilder;
        _backDatingRuleSetBuilder = backDatingRuleSetBuilder;
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<IValidationRule>[] validationRuleSets = await Task.WhenAll(
            _accountingNumberRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _accountingNameRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _letterHeadNumberRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _balanceBelowZeroRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _backDatingRuleSetBuilder.BuildAsync(formatProvider, cancellationToken));

        return validationRuleSets.SelectMany(validationRuleSet => validationRuleSet).ToArray();
    }

    #endregion
}