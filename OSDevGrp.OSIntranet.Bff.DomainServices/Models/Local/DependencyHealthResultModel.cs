namespace OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;

public class DependencyHealthResultModel : DependencyHealthModel
{
    #region Constructor

    internal DependencyHealthResultModel(string description, Uri healthEndpoint, bool isHealthy)
        : base(description, healthEndpoint)
    {
        IsHealthy = isHealthy;
    }

    #endregion

    #region Properties

    public bool IsHealthy { get; }

    #endregion
}