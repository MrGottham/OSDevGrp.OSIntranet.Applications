using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;

public class HealthMonitorResponse : ResponseBase
{
    #region Constructor

    public HealthMonitorResponse(IEnumerable<DependencyHealthResultModel> dependencies)
    {
        Dependencies = dependencies;
    }

    #endregion

    #region Properties

    public IEnumerable<DependencyHealthResultModel> Dependencies { get; }

    #endregion
}