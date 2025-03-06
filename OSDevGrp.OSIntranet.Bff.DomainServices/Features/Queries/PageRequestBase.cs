using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries;

public abstract class PageRequestBase : RequestBase
{
    #region Constructor

    protected PageRequestBase(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, securityContext)
    {
        FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
    }

    #endregion

    #region Properties

    public IFormatProvider FormatProvider { get; }

    #endregion
}