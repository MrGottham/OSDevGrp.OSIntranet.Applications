namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IPostingJournalTexts : IDynamicTexts
{
    string PostingJournalHeader { get; }

    string PostingDateHeader { get; }

    string PostingReferenceHeader { get; }

    string AccountHeader { get; }

    string AccountNameLabel { get; }

    string AccountCreditLabel { get; }

    string AccountBalanceLabel { get; }

    string AccountAvailableLabel { get; }

    string PostingTextHeader { get; }

    string BudgetAccountHeader { get; }

    string BudgetAccountNameLabel { get; }

    string BudgetAccountBudgetLabel { get; }

    string BudgetAccountPostedLabel { get; }

    string BudgetAccountAvailableLabel { get; }

    string DebitHeader { get; }

    string CreditHeader { get; }

    string PostingValueHeader { get; }

    string ContactAccountHeader { get; }

    string ContactAccountNameLabel { get; }

    string ContactAccountBalanceLabel { get; }

    int AccountingNumber { get; }

    IReadOnlyCollection<IPostingJournalLineDisplayer> PostingJournalLines { get; }

    bool Modifiable { get; }
}