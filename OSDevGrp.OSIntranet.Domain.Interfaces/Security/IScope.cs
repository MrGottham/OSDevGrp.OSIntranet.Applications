using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IScope
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<string> RelatedClaims { get; }

        IEnumerable<Claim> Filter(IEnumerable<Claim> clams);
    }
}