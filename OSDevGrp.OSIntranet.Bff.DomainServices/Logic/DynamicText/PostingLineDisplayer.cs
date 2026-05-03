using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingLineDisplayer : IPostingLineDisplayer
{
    #region Private varibales

    private readonly PostingLineModel _postingLine;
    private readonly IFormatProvider _formatProvider;

    #endregion

	#region Constructor

	private PostingLineDisplayer(PostingLineModel postingLine, IFormatProvider formatProvider)
	{
		_postingLine = postingLine;
        _formatProvider = formatProvider;
	}

	#endregion

	#region Properties

	public string Identification => _postingLine.Identifier.ToString("D", _formatProvider);

    public string PostingDate => _postingLine.PostingDate.ToLocalTime().ToString("d", _formatProvider);

    public string? PostingReference => string.IsNullOrWhiteSpace(_postingLine.Reference) == false ? _postingLine.Reference : null;

    public string Account => _postingLine.Account.AccountNumber;

    public string PostingText => _postingLine.Details;

    public string? BudgetAccount => _postingLine.BudgetAccount != null ? _postingLine.BudgetAccount.AccountNumber : null;

    public string? Debit => _postingLine.Debit != null && _postingLine.Debit != 0d ? _postingLine.Debit.Value.ToString("C", _formatProvider) : null;

    public string? Credit => _postingLine.Credit != null && _postingLine.Credit != 0d ? _postingLine.Credit.Value.ToString("C", _formatProvider) : null;

    public string? PostingValue => ResolvePostingValue();

    public string? ContactAccount => _postingLine.ContactAccount != null ? _postingLine.ContactAccount.AccountNumber : null;

    public string Summary => $"{PostingDate} {PostingText}";

	#endregion

	#region Methods

    private string? ResolvePostingValue()
    {
        double postingValue = _postingLine.Debit ?? 0d - _postingLine.Credit ?? 0d;

        return postingValue != 0d ? postingValue.ToString("C", _formatProvider) : null;
    }

	internal static IPostingLineDisplayer Create(PostingLineModel postingLine, IFormatProvider formatProvider)
	{
		return new PostingLineDisplayer(postingLine, formatProvider);
	}

	#endregion
}