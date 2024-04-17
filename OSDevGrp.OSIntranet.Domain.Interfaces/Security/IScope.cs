using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IScope
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<string> RelatedClaims { get; }
    }
}