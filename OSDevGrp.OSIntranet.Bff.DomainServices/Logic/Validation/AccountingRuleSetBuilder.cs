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
    private readonly IPostingJournalRuleSetBuilder _postingJournalRuleSetBuilder;

    #endregion

    #region Constructor

    public AccountingRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, IAccountingNumberRuleSetBuilder accountingNumberRuleSetBuilder, IAccountingNameRuleSetBuilder accountingNameRuleSetBuilder, ILetterHeadNumberRuleSetBuilder letterHeadNumberRuleSetBuilder, IBalanceBelowZeroRuleSetBuilder balanceBelowZeroRuleSetBuilder, IBackDatingRuleSetBuilder backDatingRuleSetBuilder, IPostingJournalRuleSetBuilder postingJournalRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
        _accountingNumberRuleSetBuilder = accountingNumberRuleSetBuilder;
        _accountingNameRuleSetBuilder = accountingNameRuleSetBuilder;
        _letterHeadNumberRuleSetBuilder = letterHeadNumberRuleSetBuilder;
        _balanceBelowZeroRuleSetBuilder = balanceBelowZeroRuleSetBuilder;
        _backDatingRuleSetBuilder = backDatingRuleSetBuilder;
        _postingJournalRuleSetBuilder = postingJournalRuleSetBuilder;
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
            _backDatingRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _postingJournalRuleSetBuilder.BuildAsync(formatProvider, cancellationToken));

        return validationRuleSets.SelectMany(validationRuleSet => validationRuleSet).DistinctBy(validationRule => validationRule.Name).ToArray();
    }

    #endregion
}