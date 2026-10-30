using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public sealed class ApplyPostingJournalRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public ApplyPostingJournalRequest(Guid requestId, int accountingNumber, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, securityContext)
    {
        FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
    }

    #endregion

    #region Properties

    public IFormatProvider FormatProvider { get; }

    #endregion
}