using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface ISupportedScopesProvider
    {
        IReadOnlyDictionary<string, IScope> SupportedScopes { get; }
    }
}