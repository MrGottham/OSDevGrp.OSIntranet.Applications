using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;

public class HealthMonitorRequest : RequestBase
{
    #region Constructor

    public HealthMonitorRequest(IEnumerable<DependencyHealthModel> dependencies, Guid requestId, ISecurityContext securityContext) 
        : base(requestId, securityContext)
    {
        Dependencies = dependencies;
    }

    #endregion

    #region Properties

    public IEnumerable<DependencyHealthModel> Dependencies { get; }

    #endregion
}