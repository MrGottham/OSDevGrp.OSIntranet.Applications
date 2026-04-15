using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Security.Claims;

internal static class TokenProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ITokenProvider> tokenProviderMock, Fixture fixture, IToken? token = null)
    {
        tokenProviderMock.Setup(m => m.ResolveAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(token ?? fixture.CreateToken()));
    }

    #endregion
}