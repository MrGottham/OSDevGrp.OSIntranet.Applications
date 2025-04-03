namespace OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;

public class DependencyHealthModel
{
    #region Constructor

    public DependencyHealthModel(string description, Uri healthEndpoint)
    {
        Description = description;
        HealthEndpoint = healthEndpoint;
    }

    #endregion

    #region Properties

    public string Description { get; }

    public Uri HealthEndpoint { get; }

    #endregion

    #region Methods

    public DependencyHealthResultModel GenerateResult(bool isHealthy)
    {
        return new DependencyHealthResultModel(Description, HealthEndpoint, isHealthy);
    }

    #endregion
}