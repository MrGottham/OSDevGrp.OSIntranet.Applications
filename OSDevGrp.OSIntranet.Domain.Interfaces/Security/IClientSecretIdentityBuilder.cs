using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
	public interface IClientSecretIdentityBuilder
    {
        IClientSecretIdentityBuilder WithIdentifier(int identifier);

        IClientSecretIdentityBuilder WithClientId(string clientId);

        IClientSecretIdentityBuilder WithClientSecret(string clientSecret);

        IClientSecretIdentityBuilder AddClaims(IEnumerable<Claim> claims);

        IClientSecretIdentity Build();
    }
}