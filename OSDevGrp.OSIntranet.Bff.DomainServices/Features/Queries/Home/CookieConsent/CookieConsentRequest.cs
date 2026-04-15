using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;

public class CookieConsentRequest : PageRequestBase
{
    #region Constructor

    public CookieConsentRequest(Guid requestId, string applicationName, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
        ApplicationName = applicationName;
    }

    #endregion

    #region Properties

    public string ApplicationName { get; }

    #endregion
}