using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;

public interface IUserInfoProvider
{
    bool IsAuthenticated(ClaimsPrincipal claimsPrincipal);

    Task<IUserInfoModel?> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);
}