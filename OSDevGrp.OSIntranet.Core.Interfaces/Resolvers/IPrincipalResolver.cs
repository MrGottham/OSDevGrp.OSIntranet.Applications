using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Resolvers
{
    public interface IPrincipalResolver
    {
        IPrincipal GetCurrentPrincipal();
    }
}