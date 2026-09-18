using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public abstract class PostingJournalLineDataRequestBase : PostingJournalLineIdentificationRequestBase
{
    #region Constructor

    protected PostingJournalLineDataRequestBase(
        Guid requestId,
        int accountingNumber,
        Guid identifier,
        DateTimeOffset postingDate,
        string? postingReference,
        string account,
        string postingText,
        string? budgetAccount,
        decimal? debit,
        decimal? credit,
        string? contactAccount,
        ISecurityContext securityContext)
        : base(requestId, accountingNumber, identifier, securityContext)
    {
        PostingDate = postingDate;
        PostingReference = postingReference;
        Account = account;
        PostingText = postingText;
        BudgetAccount = budgetAccount;
        Debit = debit;
        Credit = credit;
        ContactAccount = contactAccount;
    }

    #endregion

    #region Properties

    public DateTimeOffset PostingDate { get; }

    public string? PostingReference { get; }

    public string Account { get; }

    public string PostingText { get; }

    public string? BudgetAccount { get; }

    public decimal? Debit { get; }

    public decimal? Credit { get; }

    public string? ContactAccount { get; }

    #endregion
}