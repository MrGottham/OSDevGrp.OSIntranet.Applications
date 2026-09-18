using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public abstract class PostingJournalLineIdentificationRequestBase : AccountingIdentificationRequestBase
{
    #region Constructor

    protected PostingJournalLineIdentificationRequestBase(Guid requestId, int accountingNumber, Guid identifier, ISecurityContext securityContext)
        : base(requestId, accountingNumber, securityContext)
    {
        Identifier = identifier;
    }

    #endregion

    #region Properties

    public Guid Identifier { get; }

    #endregion
}