using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DependencyHealth.DependencyHealthMonitor;

internal static class DependencyHealthMonitorMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IDependencyHealthMonitor> dependencyHealthMonitorMock, Fixture fixture)
    {
        dependencyHealthMonitorMock.Setup(m => m.CheckHealthAsync(It.IsAny<DependencyHealthModel>(), It.IsAny<CancellationToken>()))
            .Returns<DependencyHealthModel, CancellationToken>((dependencyHealthModel, _) => Task.FromResult(dependencyHealthModel.GenerateResult(fixture.Create<bool>())));
    }

    #endregion
}