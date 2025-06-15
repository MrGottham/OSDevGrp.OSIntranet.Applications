using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;

internal static class SecurityContextProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ISecurityContextProvider> securityContextProviderMock, Fixture fixture, ISecurityContext? securityContext = null)
    {
        securityContextProviderMock.Setup(m => m.GetCurrentSecurityContextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(securityContext ?? fixture.CreateSecurityContext()));
    }

    #endregion
}