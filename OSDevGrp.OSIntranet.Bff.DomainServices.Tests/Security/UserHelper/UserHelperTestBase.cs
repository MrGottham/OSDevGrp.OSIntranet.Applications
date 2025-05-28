using AutoFixture;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

public abstract class UserHelperTestBase
{
    #region Methods

    protected static IPermissionChecker CreatePermissionChecker(IPermissionValidator? permissionValidator = null)
    {
        return new DomainServices.Security.UserHelper(permissionValidator ?? CreatePermissionValidator());
    }

    protected static IUserHelper CreateUserHelper(IPermissionValidator? permissionValidator = null)
    {
        return new DomainServices.Security.UserHelper(permissionValidator ?? CreatePermissionValidator());
    }

    protected static ClaimsPrincipal CreateUser(Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, bool hasEmailClaim = true, bool hasEmailClaimValue = true, string? emailClaimValue = null, bool hasAccountingClaim = true, bool hasAccountingClaimValue = true, int? accountingClaimValue = null, bool hasAccountingAdministratorClaim = true, bool hasAccountingAdministratorClaimValue = true, string? accountingAdministratorClaimValue = null, bool hasAccountingCreatorClaim = true, bool hasAccountingCreatorClaimValue = true, string? accountingCreatorClaimValue = null, bool hasAccountingModifierClaim = true, bool hasAccountingModifierClaimValue = true, string? accountingModifierClaimValue = null, bool hasAccountingViewerClaim = true, bool hasAccountingViewerClaimValue = true, string? accountingViewerClaimValue = null, bool hasCommonDataClaim = true, bool hasCommonDataClaimValue = true, string? commonDataClaimValue = null)
    {
        IList<Claim> extraClaims = new List<Claim>();
        if (hasEmailClaim)
        {
            extraClaims.Add(new Claim(ClaimTypes.Email, hasEmailClaimValue ? emailClaimValue ?? $"{fixture.Create<string>()}@{fixture.Create<string>()}.local" : string.Empty));
        }
        if (hasAccountingClaim)
        {
            extraClaims.Add(new Claim(DomainServices.Security.ClaimTypes.AccountingClaimType, hasAccountingClaimValue ? Convert.ToString(accountingClaimValue ?? fixture.Create<int>()) : string.Empty));
        }
        if (hasAccountingAdministratorClaim)
        {
            extraClaims.Add(new Claim(DomainServices.Security.ClaimTypes.AccountingAdministratorClaimType, hasAccountingAdministratorClaimValue ? accountingAdministratorClaimValue ?? fixture.Create<string>() : string.Empty));
        }
        if (hasAccountingCreatorClaim)
        {
            extraClaims.Add(new Claim(DomainServices.Security.ClaimTypes.AccountingCreatorClaimType, hasAccountingCreatorClaimValue ? accountingCreatorClaimValue ?? fixture.Create<string>() : string.Empty));
        }
        if (hasAccountingModifierClaim)
        {
            extraClaims.Add(new Claim(DomainServices.Security.ClaimTypes.AccountingModifierClaimType, hasAccountingModifierClaimValue ? accountingModifierClaimValue ?? $"{string.Join(',', fixture.CreateMany<int>(3).ToArray())},*" : string.Empty));
        }
        if (hasAccountingViewerClaim)
        {
            extraClaims.Add(new Claim(DomainServices.Security.ClaimTypes.AccountingViewerClaimType, hasAccountingViewerClaimValue ? accountingViewerClaimValue ?? $"{string.Join(',', fixture.CreateMany<int>(3).ToArray())},*" : string.Empty));
        }
        if (hasCommonDataClaim)
        {
            extraClaims.Add(new Claim(DomainServices.Security.ClaimTypes.CommonDataClaimType, hasCommonDataClaimValue ? commonDataClaimValue ?? fixture.Create<string>() : string.Empty));
        }

        ClaimsIdentity claimsIdentity = fixture.CreateAuthenticatedClaimsIdentity(
            hasNameIdentifierClaim: hasNameIdentifierClaim,
            hasNameIdentifierClaimValue: hasNameIdentifierClaimValue,
            nameIdentifierClaimValue: nameIdentifierClaimValue,
            hasNameClaim: hasNameClaim,
            hasNameClaimValue: hasNameClaimValue,
            nameClaimValue: nameClaimValue,
            extraClaims: extraClaims.ToArray());

        return fixture.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
    }

    private static IPermissionValidator CreatePermissionValidator()
    {
        return new DomainServices.Security.PermissionValidator();
    }

    #endregion
}