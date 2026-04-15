using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class UserHelper : IUserHelper
{
    #region Private variables

    private readonly IPermissionValidator _permissionValidator;

    #endregion

    #region Constructor

    public UserHelper(IPermissionValidator permissionValidator)
    {
        _permissionValidator = permissionValidator;
    }

    #endregion

    #region Methods

    public string? GetNameIdentifier(ClaimsPrincipal user)
    {
        return GetClaimValue(user, System.Security.Claims.ClaimTypes.NameIdentifier);
    }

    public string? GetFullName(ClaimsPrincipal user)
    {
        string? claimValue = GetClaimValue(user, System.Security.Claims.ClaimTypes.Name);
        if (string.IsNullOrWhiteSpace(claimValue) == false)
        {
            return claimValue;
        }

        return GetMailAddress(user);
    }

    public string? GetMailAddress(ClaimsPrincipal user)
    {
        return GetClaimValue(user, System.Security.Claims.ClaimTypes.Email);
    }

    public bool IsAuthenticated(ClaimsPrincipal user)
    {
        return _permissionValidator.IsAuthenticated(user);
    }

    public bool HasAccountingAccess(ClaimsPrincipal user)
    {
        return HasClaim(user, ClaimTypes.AccountingClaimType);
    }

    public int? GetDefaultAccountingNumber(ClaimsPrincipal user)
    {
        string? claimValue = GetClaimValue(user, ClaimTypes.AccountingClaimType);
        if (string.IsNullOrWhiteSpace(claimValue))
        {
            return null;
        }

        if (int.TryParse(claimValue, CultureInfo.InvariantCulture, out int accountingNumber))
        {
            return accountingNumber;
        }

        return null;
    }

    public bool IsAccountingAdministrator(ClaimsPrincipal user)
    {
        return HasAccountingAccess(user) && HasClaim(user, ClaimTypes.AccountingAdministratorClaimType);
    }

    public bool IsAccountingCreator(ClaimsPrincipal user)
    {
        return HasAccountingAccess(user) && HasClaim(user, ClaimTypes.AccountingCreatorClaimType);
    }

    public bool IsAccountingModifier(ClaimsPrincipal user, int? accountingNumber = null)
    {
        if (HasAccountingAccess(user) == false)
        {
            return false;
        }

        if (accountingNumber == null)
        {
            return HasClaim(user, ClaimTypes.AccountingModifierClaimType);
        }

        return IsClaimValueAllowingAccountingNumber(user, ClaimTypes.AccountingModifierClaimType, accountingNumber.Value);
    }

    public bool IsAccountingViewer(ClaimsPrincipal user, int? accountingNumber = null)
    {
        if (HasAccountingAccess(user) == false)
        {
            return false;
        }

        if (accountingNumber == null)
        {
            return HasClaim(user, ClaimTypes.AccountingViewerClaimType);
        }

        return IsClaimValueAllowingAccountingNumber(user, ClaimTypes.AccountingViewerClaimType, accountingNumber.Value);
    }

    public bool HasCommonDataAccess(ClaimsPrincipal user)
    {
        return HasClaim(user, ClaimTypes.CommonDataClaimType);
    }

    private bool HasClaim(ClaimsPrincipal user, string claimType)
    {
        return _permissionValidator.HasClaim(user, claim => claim.Type == claimType);
    }

    private static string? GetClaimValue(ClaimsPrincipal user, string claimType)
    {
        Claim? claim = user.FindFirst(claimType);
        if (claim == null)
        {
            return null;
        }

        return string.IsNullOrWhiteSpace(claim.Value) ? null : claim.Value;
    }

    private static bool IsClaimValueAllowingAccountingNumber(ClaimsPrincipal user, string claimType, int accountingNumber)
    {
        string? claimValue = GetClaimValue(user, claimType);
        if (string.IsNullOrWhiteSpace(claimValue))
        {
            return false;
        }

        string[] allowedValues = claimValue.Split([','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Where(allowedValue => string.IsNullOrWhiteSpace(allowedValue) == false)
            .ToArray();

        return allowedValues.Contains("*") || allowedValues.Contains(accountingNumber.ToString(CultureInfo.InvariantCulture));
    }

    #endregion
}