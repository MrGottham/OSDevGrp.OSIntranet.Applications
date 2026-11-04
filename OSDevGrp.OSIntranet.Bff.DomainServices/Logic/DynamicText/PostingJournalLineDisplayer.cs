using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingJournalLineDisplayer : IPostingJournalLineDisplayer
{
    #region Private variables

    private readonly ApplyPostingLineModel _postingJournalLine;
    private readonly IFormatProvider _formatProvider;

    #endregion

    #region Constructor

    private PostingJournalLineDisplayer(ApplyPostingLineModel postingJournalLine, IFormatProvider formatProvider)
    {
        _postingJournalLine = postingJournalLine;
        _formatProvider = formatProvider;
    }

    #endregion

    #region Properties

    public string Identification => _postingJournalLine.Identifier.HasValue ? _postingJournalLine.Identifier.Value.ToString("D", _formatProvider) : string.Empty;

    public string PostingDate => _postingJournalLine.PostingDate.ToLocalTime().ToString("d", _formatProvider);

    public string? PostingReference => string.IsNullOrWhiteSpace(_postingJournalLine.Reference) == false ? _postingJournalLine.Reference : null;

    public string Account => _postingJournalLine.AccountNumber;

    public string PostingText => _postingJournalLine.Details;

    public string? BudgetAccount => string.IsNullOrWhiteSpace(_postingJournalLine.BudgetAccountNumber) == false ? _postingJournalLine.BudgetAccountNumber : null;

    public string? Debit => _postingJournalLine.Debit != null && _postingJournalLine.Debit != 0d ? _postingJournalLine.Debit.Value.ToString("C", _formatProvider) : null;

    public string? Credit => _postingJournalLine.Credit != null && _postingJournalLine.Credit != 0d ? _postingJournalLine.Credit.Value.ToString("C", _formatProvider) : null;

    public string? PostingValue => ResolvePostingValue();

    public string? ContactAccount => string.IsNullOrWhiteSpace(_postingJournalLine.ContactAccountNumber) == false ? _postingJournalLine.ContactAccountNumber : null;

    public ApplyPostingLineModel PostingJournalLine => _postingJournalLine;

    #endregion

    #region Methods

    private string? ResolvePostingValue()
    {
        double postingValue = _postingJournalLine.Debit ?? 0d - _postingJournalLine.Credit ?? 0d;

        return postingValue != 0d ? postingValue.ToString("C", _formatProvider) : null;
    }

    internal static IPostingJournalLineDisplayer Create(ApplyPostingLineModel postingJournalLine, IFormatProvider formatProvider)
    {
        return new PostingJournalLineDisplayer(postingJournalLine, formatProvider);
    }

    #endregion
}