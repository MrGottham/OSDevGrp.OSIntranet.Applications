using System;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Resolvers
{
    public interface ITrustedDomainResolver
    {
        bool IsTrustedDomain(Uri uri);
    }
}