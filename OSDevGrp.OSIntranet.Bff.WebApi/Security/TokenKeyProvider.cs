using Microsoft.Extensions.Options;
using System.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class TokenKeyProvider : ITokenKeyProvider
{
    #region Private variables

    private readonly ITokenKeyGenerator _tokenKeyGenerator;
    private readonly IOptions<TokenKeyProviderOptions> _tokenKeyProviderOptions;

    #endregion

    #region Constructor

    public TokenKeyProvider(ITokenKeyGenerator tokenKeyGenerator, IOptions<TokenKeyProviderOptions> tokenKeyProviderOptions)
    {
        _tokenKeyGenerator = tokenKeyGenerator;
        _tokenKeyProviderOptions = tokenKeyProviderOptions;
    }

    #endregion

    #region Methods

    public async Task<string> ResolveAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        if (claimsPrincipal.Identity == null || claimsPrincipal.Identity.IsAuthenticated == false)
        {
            return await _tokenKeyGenerator.GenerateAsync(GenerateValuesForNonAuthenticatedClaimsPrincipal(_tokenKeyProviderOptions.Value), cancellationToken);
        }

        return await _tokenKeyGenerator.GenerateAsync(GenerateValuesForAuthenticatedClaimsIdentity(_tokenKeyProviderOptions.Value, (ClaimsIdentity) claimsPrincipal.Identity), cancellationToken);
    }

    private static IReadOnlyCollection<string> GenerateValuesForNonAuthenticatedClaimsPrincipal(TokenKeyProviderOptions tokenKeyProviderOptions)
    {
        return
        [
            tokenKeyProviderOptions.TokenStorageType.Namespace ?? string.Empty,
            tokenKeyProviderOptions.TokenStorageType.Name,
            tokenKeyProviderOptions.AnonymousUserIdentifier,
            tokenKeyProviderOptions.Salt
        ];
    }

    private static IReadOnlyCollection<string> GenerateValuesForAuthenticatedClaimsIdentity(TokenKeyProviderOptions tokenKeyProviderOptions, ClaimsIdentity claimsIdentity)
    {
        Claim? nameIdentifierClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claimsIdentity.IsAuthenticated == false || string.IsNullOrWhiteSpace(nameIdentifierClaim?.Value))
        {
            throw new SecurityException("Unable to generate a token key for the authenticated user.");
        }

        IList<string> values = new List<string>
        {
            tokenKeyProviderOptions.TokenStorageType.Namespace ?? string.Empty,
            tokenKeyProviderOptions.TokenStorageType.Name,
            nameIdentifierClaim.Value,
        };

        Claim? nameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
        if (string.IsNullOrWhiteSpace(nameClaim?.Value) == false)
        {
            values.Add(nameClaim.Value);
        }

        Claim? emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(emailClaim?.Value) == false)
        {
            values.Add(emailClaim.Value);
        }

        values.Add(tokenKeyProviderOptions.Salt);

        return values.AsReadOnly();
    }

    #endregion
}