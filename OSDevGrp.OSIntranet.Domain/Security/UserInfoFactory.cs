using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class UserInfoFactory : IUserInfoFactory
    {
        #region Methods

        public IUserInfo FromPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal));

            return new UserInfo(claimsPrincipal);
        }

        #endregion
    }
}