using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public abstract class PostingJournalLineIdentificationRequestBase : AccountingIdentificationRequestBase
{
    #region Constructor

    protected PostingJournalLineIdentificationRequestBase(Guid requestId, int accountingNumber, Guid identifier, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, securityContext)
    {
        Identifier = identifier;
        FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
    }

    #endregion

    #region Properties

    public Guid Identifier { get; }

    public IFormatProvider FormatProvider { get; }

    #endregion
}