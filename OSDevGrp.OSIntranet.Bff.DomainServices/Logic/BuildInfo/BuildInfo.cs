using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.BuildInfo;

internal class BuildInfo : IBuildInfo
{
    #region Constructor

    public BuildInfo(string assembly, DateTimeOffset buildTime)
    {
        Assembly = assembly;
        BuildTime = buildTime;
    }

    #endregion

    #region Properties

    public string Assembly { get; }

    public DateTimeOffset BuildTime { get; }

    #endregion
}