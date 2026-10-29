using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;

public class PostingJournalRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public PostingJournalRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}