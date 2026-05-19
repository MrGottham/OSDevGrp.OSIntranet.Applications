namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IPostingJournalTexts : IDynamicTexts
{
    string PostingJournalHeader { get; }

    string PostingDateHeader { get; }

    string PostingReferenceHeader { get; }

    string AccountHeader { get; }

    string PostingTextHeader { get; }

    string BudgetAccountHeader { get; }

    string DebitHeader { get; }

    string CreditHeader { get; }

    string PostingValueHeader { get; }

    string ContactAccountHeader { get; }

    int AccountingNumber { get; }

    IReadOnlyCollection<IPostingJournalLineDisplayer> PostingJournalLines { get; }

    bool Modifiable { get; }
}