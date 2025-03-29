using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;

internal static class SecurityContextProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ISecurityContextProvider> securityContextProviderMock, Fixture fixture, Random random, ISecurityContext? securityContext = null)
    {
        securityContextProviderMock.Setup(m => m.GetCurrentSecurityContextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(securityContext ?? fixture.CreateSecurityContext(random)));
    }

    #endregion
}