using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.BuildInfo.BuildInfoProvider;

internal static class FixtureExtensions
{
    #region Methods

    internal static IBuildInfo CreateBuildInfo(this Fixture fixture, Random random, string? assembly = null, DateTimeOffset? buildTime = null)
    {
        return fixture.CreateBuildInfoMock(random, assembly, buildTime).Object;
    }

    internal static Mock<IBuildInfo> CreateBuildInfoMock(this Fixture fixture, Random random, string? assembly = null, DateTimeOffset? buildTime = null)
    {
        Mock<IBuildInfo> buildInfoMock = new Mock<IBuildInfo>();
        buildInfoMock.Setup(m => m.Assembly)
            .Returns(assembly ?? fixture.Create<string>());
        buildInfoMock.Setup(m => m.BuildTime)
            .Returns(buildTime ?? DateTimeOffset.UtcNow.AddSeconds(random.Next(0, (int) TimeSpan.FromMinutes(300).TotalSeconds) * -1));
        return buildInfoMock;
    }

    #endregion
}