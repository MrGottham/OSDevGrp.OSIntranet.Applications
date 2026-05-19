using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingJournalTexts : DynamicTextsBase<ApplyPostingJournalModel>, IPostingJournalTexts
{
    #region Constructor

    public PostingJournalTexts(ApplyPostingJournalModel postingJournal, string postingJournalHeader, string postingDateHeader, string postingReferenceHeader, string accountHeader, string postingTextHeader, string budgetAccountHeader, string debitHeader, string creditHeader, string postingValueHeader, string contactAccountHeader, bool modifiable, IFormatProvider formatProvider)
        : base(postingJournal, formatProvider)
    {
        PostingJournalHeader = postingJournalHeader;
        PostingDateHeader = postingDateHeader;
        PostingReferenceHeader = postingReferenceHeader;
        AccountHeader = accountHeader;
        PostingTextHeader = postingTextHeader;
        BudgetAccountHeader = budgetAccountHeader;
        DebitHeader = debitHeader;
        CreditHeader = creditHeader;
        PostingValueHeader = postingValueHeader;
        ContactAccountHeader = contactAccountHeader;
        Modifiable = modifiable;
    }

    #endregion

    #region Properties

    public string PostingJournalHeader { get; }

    public string PostingDateHeader { get; }

    public string PostingReferenceHeader { get; }

    public string AccountHeader { get; }

    public string PostingTextHeader { get; }

    public string BudgetAccountHeader { get; }

    public string DebitHeader { get; }

    public string CreditHeader { get; }

    public string PostingValueHeader { get; }

    public string ContactAccountHeader { get; }

    public int AccountingNumber => Model.AccountingNumber;

    public IReadOnlyCollection<IPostingJournalLineDisplayer> PostingJournalLines => Model.ApplyPostingLines
        .OrderByDescending(postingLine => postingLine.PostingDate)
        .ThenByDescending(postingLine => postingLine.SortOrder)
        .Select(postingLine => PostingJournalLineDisplayer.Create(postingLine, FormatProvider))
        .ToArray();

    public bool Modifiable { get; }

    #endregion
}