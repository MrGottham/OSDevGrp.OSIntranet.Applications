using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;

public interface IBuildInfoProvider
{
    IBuildInfo GetBuildInfo(Assembly assembly);
}