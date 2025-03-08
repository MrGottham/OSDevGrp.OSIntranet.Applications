using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.BuildInfo.BuildInfoProvider;

internal static class BuildInfoProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IBuildInfoProvider> buildInfoProviderMock, Fixture fixture, Random random, IBuildInfo? buildInfo = null)
    {
        buildInfoProviderMock.Setup(m => m.GetBuildInfo(It.IsAny<Assembly>()))
            .Returns(buildInfo ?? fixture.CreateBuildInfo(random));
    }

    #endregion
}