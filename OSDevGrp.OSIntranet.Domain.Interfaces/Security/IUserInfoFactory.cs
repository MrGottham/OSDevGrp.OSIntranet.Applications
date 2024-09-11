using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserInfoFactory
    {
        IUserInfo FromPrincipal(ClaimsPrincipal claimsPrincipal);
    }
}