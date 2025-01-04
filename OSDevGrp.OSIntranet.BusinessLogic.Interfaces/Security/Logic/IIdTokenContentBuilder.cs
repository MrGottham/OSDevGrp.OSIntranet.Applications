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

        IIdTokenContentBuilder WithCustomClaim(string claimType, string value);

        IEnumerable<Claim> Build();
    }
}