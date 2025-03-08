namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;

public interface IBuildInfo
{
    string Assembly { get; }

    DateTimeOffset BuildTime { get; }
}