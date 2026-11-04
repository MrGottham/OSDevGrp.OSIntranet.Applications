using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PostingJournalRuleSetBuilder : ValidationRuleSetBuilderBase, IPostingJournalRuleSetBuilder
{
    #region Private variables

    private readonly IAccountingNumberRuleSetBuilder _accountingNumberRuleSetBuilder;
    private readonly IPostingJournalLineIdentifierRuleSetBuilder _postingJournalLineIdentifierRuleSetBuilder;
    private readonly IPostingDateRuleSetBuilder _postingDateRuleSetBuilder;
    private readonly IPostingReferenceRuleSetBuilder _postingReferenceRuleSetBuilder;
    private readonly IAccountNumberRuleSetBuilder _accountNumberRuleSetBuilder;
    private readonly IPostingTextRuleSetBuilder _postingTextRuleSetBuilder;
    private readonly IBudgetAccountNumberRuleSetBuilder _budgetAccountNumberRuleSetBuilder;
    private readonly IDebitRuleSetBuilder _debitRuleSetBuilder;
    private readonly ICreditRuleSetBuilder _creditRuleSetBuilder;
    private readonly IContactAccountNumberRuleSetBuilder _contactAccountNumberRuleSetBuilder;

    #endregion

    #region Constructor

    public PostingJournalRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, IAccountingNumberRuleSetBuilder accountingNumberRuleSetBuilder, IPostingJournalLineIdentifierRuleSetBuilder postingJournalLineIdentifierRuleSetBuilder, IPostingDateRuleSetBuilder postingDateRuleSetBuilder, IPostingReferenceRuleSetBuilder postingReferenceRuleSetBuilder, IAccountNumberRuleSetBuilder accountNumberRuleSetBuilder, IPostingTextRuleSetBuilder postingTextRuleSetBuilder, IBudgetAccountNumberRuleSetBuilder budgetAccountNumberRuleSetBuilder, IDebitRuleSetBuilder debitRuleSetBuilder, ICreditRuleSetBuilder creditRuleSetBuilder, IContactAccountNumberRuleSetBuilder contactAccountNumberRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)
    {
        _accountingNumberRuleSetBuilder = accountingNumberRuleSetBuilder;
        _postingJournalLineIdentifierRuleSetBuilder = postingJournalLineIdentifierRuleSetBuilder;
        _postingDateRuleSetBuilder = postingDateRuleSetBuilder;
        _postingReferenceRuleSetBuilder = postingReferenceRuleSetBuilder;
        _accountNumberRuleSetBuilder = accountNumberRuleSetBuilder;
        _postingTextRuleSetBuilder = postingTextRuleSetBuilder;
        _budgetAccountNumberRuleSetBuilder = budgetAccountNumberRuleSetBuilder;
        _debitRuleSetBuilder = debitRuleSetBuilder;
        _creditRuleSetBuilder = creditRuleSetBuilder;
        _contactAccountNumberRuleSetBuilder = contactAccountNumberRuleSetBuilder;
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<IValidationRule>[] validationRuleSets = await Task.WhenAll(
            _accountingNumberRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _postingJournalLineIdentifierRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _postingDateRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _postingReferenceRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _accountNumberRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _postingTextRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _budgetAccountNumberRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _debitRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _creditRuleSetBuilder.BuildAsync(formatProvider, cancellationToken),
            _contactAccountNumberRuleSetBuilder.BuildAsync(formatProvider, cancellationToken));

        return validationRuleSets.SelectMany(validationRuleSet => validationRuleSet).DistinctBy(validationRule => validationRule.Name).ToArray();
    }

    #endregion
}