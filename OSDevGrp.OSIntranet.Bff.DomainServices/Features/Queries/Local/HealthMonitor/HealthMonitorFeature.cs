using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;

internal class HealthMonitorFeature : IQueryFeature<HealthMonitorRequest, HealthMonitorResponse>
{
    #region Private variables

    private readonly IDependencyHealthMonitor _dependencyHealthMonitor;

    #endregion

    #region Constructor

    public HealthMonitorFeature(IDependencyHealthMonitor dependencyHealthMonitor)
    {
        _dependencyHealthMonitor = dependencyHealthMonitor;
    }

    #endregion

    #region Methods

    public async Task<HealthMonitorResponse> ExecuteAsync(HealthMonitorRequest request, CancellationToken cancellationToken = default)
    {
        DependencyHealthResultModel[] dependenciesHealthResults = await Task.WhenAll(request.Dependencies.Select(dependency => _dependencyHealthMonitor.CheckHealthAsync(dependency, cancellationToken)));

        return new HealthMonitorResponse(dependenciesHealthResults.OrderBy(dependencyHealthResult => dependencyHealthResult.Description).ToArray());
    }

    #endregion
}