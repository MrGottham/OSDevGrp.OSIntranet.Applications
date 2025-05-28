using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IPermissionChecker
{
    bool IsAuthenticated(ClaimsPrincipal user);

    bool HasAccountingAccess(ClaimsPrincipal user);

    bool IsAccountingAdministrator(ClaimsPrincipal user);

    bool IsAccountingCreator(ClaimsPrincipal user);

    bool IsAccountingModifier(ClaimsPrincipal user, int? accountingNumber = null);

    bool IsAccountingViewer(ClaimsPrincipal user, int? accountingNumber = null);

    bool HasCommonDataAccess(ClaimsPrincipal user);
}