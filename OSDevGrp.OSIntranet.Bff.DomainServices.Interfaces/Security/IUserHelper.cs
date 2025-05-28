using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IUserHelper : IPermissionChecker
{
    string? GetNameIdentifier(ClaimsPrincipal user);

    string? GetFullName(ClaimsPrincipal user);

    string? GetMailAddress(ClaimsPrincipal user);

    int? GetDefaultAccountingNumber(ClaimsPrincipal user);
}