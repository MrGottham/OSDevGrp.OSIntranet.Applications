using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingLineCollectionTexts : DynamicTextsBase<IReadOnlyCollection<PostingLineModel>>, IPostingLineCollectionTexts
{
    #region Constructor

    public PostingLineCollectionTexts(IReadOnlyCollection<PostingLineModel> postingLines, string latestPostingsHeader, string postingDateHeader, string postingReferenceHeader, string accountHeader, string postingTextHeader, string budgetAccountHeader, string debitHeader, string creditHeader, string postingValueHeader, string contactAccountHeader, string summaryHeader, IFormatProvider formatProvider)
        : base(postingLines, formatProvider)
    {
        LatestPostingsHeader = latestPostingsHeader;
        PostingDateHeader = postingDateHeader;
        PostingReferenceHeader = postingReferenceHeader;
        SummaryHeader = summaryHeader;
        AccountHeader = accountHeader;
        PostingTextHeader = postingTextHeader;
        BudgetAccountHeader = budgetAccountHeader;
        DebitHeader = debitHeader;
        CreditHeader = creditHeader;
        PostingValueHeader = postingValueHeader;
        ContactAccountHeader = contactAccountHeader;
        SummaryHeader = summaryHeader;
    }

    #endregion

    #region Properties

    public string LatestPostingsHeader { get; }

    public string PostingDateHeader { get; }

    public string PostingReferenceHeader { get; }

    public string AccountHeader { get; }

    public string PostingTextHeader { get; }

    public string BudgetAccountHeader { get; }

    public string DebitHeader { get; }

    public string CreditHeader { get; }

    public string PostingValueHeader { get; }

    public string ContactAccountHeader { get; }

    public string SummaryHeader { get; }

    public IReadOnlyCollection<IPostingLineDisplayer> PostingLines => Model.OrderByDescending(postingLine => postingLine.PostingDate)
        .ThenByDescending(postingLine => postingLine.SortOrder)
        .Select(postingLine => PostingLineDisplayer.Create(postingLine, FormatProvider))
        .ToArray();

    #endregion
}