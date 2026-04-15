namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;

public interface IDependencyHealthMonitor
{
    Task<DependencyHealthResultModel> CheckHealthAsync(DependencyHealthModel dependencyHealthModel, CancellationToken cancellationToken = default);
}