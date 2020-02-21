using System;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    public interface ITrustedDomainHelper
    {
        bool IsTrustedDomain(Uri uri);
    }
}