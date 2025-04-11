using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using System.Collections.Concurrent;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.BuildInfo;

internal class BuildInfoProvider : IBuildInfoProvider
{
    #region Private variables

    private readonly IDictionary<string, IBuildInfo> _buildInfos = new ConcurrentDictionary<string, IBuildInfo>();
    private readonly object _syncRoot = new object();

    #endregion

    #region Methods

    public IBuildInfo GetBuildInfo(Assembly assembly)
    {
        lock (_syncRoot)
        {
            string assemblyName = assembly.GetName().FullName;
            if (_buildInfos.TryGetValue(assemblyName, out IBuildInfo? buildInfo))
            {
                return buildInfo;
            }

            DateTimeOffset buildTime = GetBuildTime(assembly);

            buildInfo = new BuildInfo(assemblyName, buildTime);
            _buildInfos.Add(assemblyName, buildInfo);

            return buildInfo;
        }
    }

    private static DateTimeOffset GetBuildTime(Assembly assembly)
    {
        return GetBuildTime(assembly.Location);
    }

    private static DateTimeOffset GetBuildTime(string assemblyLocation)
    {
        FileInfo assemblyFileInfo = new FileInfo(assemblyLocation);
        if (assemblyFileInfo.Exists == false)
        {
            throw new FileNotFoundException($"The assembly was not found: {assemblyLocation}", assemblyLocation);
        }
        return GetBuildTime(assemblyFileInfo);
    }

    private static DateTimeOffset GetBuildTime(FileInfo assemblyFileInfo)
    {
        return new DateTimeOffset(File.GetCreationTimeUtc(assemblyFileInfo.FullName), TimeSpan.Zero);
    }

    #endregion
}