using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security;
using System.Security.Claims;

internal static class TokenProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ITokenProvider> tokenProviderMock, Fixture fixture, Random random, IToken? token = null)
    {
        tokenProviderMock.Setup(m => m.ResolveAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(token ?? fixture.CreateToken(random)));
    }

    #endregion
}