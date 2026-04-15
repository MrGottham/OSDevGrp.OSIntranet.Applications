using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal static class CookieSigningInContextExtensions
{
    #region Private varriables

    private static readonly string[] WellknownClaimTypes =
    {
        ClaimTypes.NameIdentifier,
        ClaimTypes.Name,
        ClaimTypes.Email,
        DomainServices.Security.ClaimTypes.AccountingClaimType,
        DomainServices.Security.ClaimTypes.AccountingAdministratorClaimType,
        DomainServices.Security.ClaimTypes.AccountingCreatorClaimType,
        DomainServices.Security.ClaimTypes.AccountingModifierClaimType,
        DomainServices.Security.ClaimTypes.AccountingViewerClaimType,
        DomainServices.Security.ClaimTypes.CommonDataClaimType
    };

    #endregion

    #region Methods

    internal static Task TransformAsync(this CookieSigningInContext context)
    {
        return Task.Run(() =>
        {
            context.Principal = context.Principal?.Transform();

            DateTimeOffset? expiresAt = null;
            context.Properties?.CleanUp(out expiresAt);

            if (expiresAt.HasValue)
            {
                context.CookieOptions.Expires = expiresAt.Value.UtcDateTime;
            }
        });
    }

    private static ClaimsPrincipal Transform(this ClaimsPrincipal claimsPrincipal)
    {
        IEnumerable<Claim> claims = claimsPrincipal.Claims.Transform();

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, claimsPrincipal.Identity?.AuthenticationType);

        return new ClaimsPrincipal(claimsIdentity);
    }

    private static Claim[] Transform(this IEnumerable<Claim> claims)
    {
        return claims.Where(claim => WellknownClaimTypes.Contains(claim.Type)).ToArray();
    }

    private static void CleanUp(this AuthenticationProperties authenticationProperties, out DateTimeOffset? expiresAt)
    {
        expiresAt = authenticationProperties.ExpiresUtc;

        if (authenticationProperties.Items.TryGetValue(".Token.expires_at", out string? value))
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                expiresAt = DateTimeOffset.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        authenticationProperties.Items.Clear();
    }

    #endregion
}