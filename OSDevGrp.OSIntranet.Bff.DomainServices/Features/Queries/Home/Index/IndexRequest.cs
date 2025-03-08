using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;

public class IndexRequest : PageRequestBase
{
    #region Constructor

    public IndexRequest(Guid requestId, Assembly executingAssembly, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, formatProvider, securityContext)
    {
        ExecutingAssembly = executingAssembly;
    }

    #endregion

    #region Properties

    public Assembly ExecutingAssembly { get; }

    #endregion
}