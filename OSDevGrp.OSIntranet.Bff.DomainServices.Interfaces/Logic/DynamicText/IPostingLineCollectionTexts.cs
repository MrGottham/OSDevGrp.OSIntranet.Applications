namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IPostingLineCollectionTexts : IDynamicTexts
{
    string LatestPostingsHeader { get; }

    string PostingDateHeader { get; }

    string PostingReferenceHeader { get; }

    string AccountHeader { get; }

    string PostingTextHeader { get; }

    string BudgetAccountHeader { get; }

    string DebitHeader { get; }

    string CreditHeader { get; }

    string PostingValueHeader { get; }

    string ContactAccountHeader { get; }

    string SummaryHeader { get; }

    IReadOnlyCollection<IPostingLineDisplayer> PostingLines { get; }
}