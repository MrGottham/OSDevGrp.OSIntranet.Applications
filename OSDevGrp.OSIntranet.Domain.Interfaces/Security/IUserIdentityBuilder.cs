using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
	public interface IUserIdentityBuilder
    {
        IUserIdentityBuilder WithIdentifier(int identifier);

        IUserIdentityBuilder AddClaims(IEnumerable<Claim> claims);

        IUserIdentity Build();
    }
}