using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;

public class UserInfoRequest : PageRequestBase
{
    #region Constructor

    public UserInfoRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
    }

    #endregion

    #region Properties

    public ClaimsPrincipal User => SecurityContext.User;

    #endregion
}