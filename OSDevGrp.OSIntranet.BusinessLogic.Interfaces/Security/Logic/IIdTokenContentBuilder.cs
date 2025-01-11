using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IIdTokenContentBuilder
    {
        IIdTokenContentBuilder WithNonce(string nonce);

        IIdTokenContentBuilder WithAuthenticationContextClassReference(string authenticationContextClassReference);

        IIdTokenContentBuilder WithAuthenticationMethodsReferences(IEnumerable<string> authenticationMethodsReferences);

        IIdTokenContentBuilder WithAuthorizedParty(string authorizedParty);

        IIdTokenContentBuilder WithCustomClaimsFilteredByScope(IScope scope, IEnumerable<Claim> customClaims);

        IIdTokenContentBuilder WithCustomClaimsFilteredByClaimType(string claimType, IEnumerable<Claim> customClaims);

        IEnumerable<Claim> Build();
    }
}