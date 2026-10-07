using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public sealed class ModifyPostingLineInPostingJournalRequest : PostingJournalLineDataRequestBase
{
    #region Constructor

    public ModifyPostingLineInPostingJournalRequest(
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
        : base(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext)
    {
    }

    #endregion
}