using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingJournalTexts : DynamicTextsBase<ApplyPostingJournalModel>, IPostingJournalTexts
{
    #region Constructor

    public PostingJournalTexts(ApplyPostingJournalModel postingJournal, string postingJournalHeader, string postingDateHeader, string postingReferenceHeader, string accountHeader, string accountNameLabel, string accountCreditLabel, string accountBalanceLabel, string accountAvailableLabel, string postingTextHeader, string budgetAccountHeader, string budgetAccountNameLabel, string budgetAccountBudgetLabel, string budgetAccountPostedLabel, string budgetAccountAvailableLabel, string debitHeader, string creditHeader, string postingValueHeader, string contactAccountHeader, string contactAccountNameLabel, string contactAccountBalanceLabel, bool modifiable, IFormatProvider formatProvider)
        : base(postingJournal, formatProvider)
    {
        PostingJournalHeader = postingJournalHeader;
        PostingDateHeader = postingDateHeader;
        PostingReferenceHeader = postingReferenceHeader;
        AccountHeader = accountHeader;
        AccountNameLabel = accountNameLabel;
        AccountCreditLabel = accountCreditLabel;
        AccountBalanceLabel = accountBalanceLabel;
        AccountAvailableLabel = accountAvailableLabel;
        PostingTextHeader = postingTextHeader;
        BudgetAccountHeader = budgetAccountHeader;
        BudgetAccountNameLabel = budgetAccountNameLabel;
        BudgetAccountBudgetLabel = budgetAccountBudgetLabel;
        BudgetAccountPostedLabel = budgetAccountPostedLabel;
        BudgetAccountAvailableLabel = budgetAccountAvailableLabel;
        DebitHeader = debitHeader;
        CreditHeader = creditHeader;
        PostingValueHeader = postingValueHeader;
        ContactAccountHeader = contactAccountHeader;
        ContactAccountNameLabel = contactAccountNameLabel;
        ContactAccountBalanceLabel = contactAccountBalanceLabel;
        Modifiable = modifiable;
    }

    #endregion

    #region Properties

    public string PostingJournalHeader { get; }

    public string PostingDateHeader { get; }

    public string PostingReferenceHeader { get; }

    public string AccountHeader { get; }

    public string AccountNameLabel { get; }

    public string AccountCreditLabel { get; }

    public string AccountBalanceLabel { get; }

    public string AccountAvailableLabel { get; }

    public string PostingTextHeader { get; }

    public string BudgetAccountHeader { get; }

    public string BudgetAccountNameLabel { get; }

    public string BudgetAccountBudgetLabel { get; }

    public string BudgetAccountPostedLabel { get; }

    public string BudgetAccountAvailableLabel { get; }

    public string DebitHeader { get; }

    public string CreditHeader { get; }

    public string PostingValueHeader { get; }

    public string ContactAccountHeader { get; }

    public string ContactAccountNameLabel { get; }

    public string ContactAccountBalanceLabel { get; }

    public int AccountingNumber => Model.AccountingNumber;

    public IReadOnlyCollection<IPostingJournalLineDisplayer> PostingJournalLines => Model.ApplyPostingLines
        .OrderByDescending(postingLine => postingLine.PostingDate)
        .ThenByDescending(postingLine => postingLine.SortOrder)
        .Select(postingLine => PostingJournalLineDisplayer.Create(postingLine, FormatProvider))
        .ToArray();

    public bool Modifiable { get; }

    #endregion
}